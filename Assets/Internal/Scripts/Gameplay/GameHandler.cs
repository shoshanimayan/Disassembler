using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Threading.Tasks;
using General;
using UI;
using Animation;
using DG.Tweening;

namespace Gameplay
{
	public class GameHandler: Singleton<GameHandler>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] private Interactable[] _interactables=new Interactable[] { };
		[SerializeField] private RotationRing _rotationRing;
		[SerializeField] private float _gameLength;
		[SerializeField] private GameObject[] _interactableSpots;
		[SerializeField] private MeshRenderer _robotRenderer;
		[SerializeField] private Material[] _robotMaterials;
		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private GameStateController _gameState { get { return GameStateController.Instance; } }
		private GameUIHandler _gameUIHandler { get { return GameUIHandler.Instance; } }
		private AnimationController _animationController { get { return AnimationController.Instance; } }
		private AudioManager _audioManager { get { return AudioManager.Instance; } }

		private int _score=0;
		private float _currentTime;
		private bool _playing;
		private int _interactionsLeft = -1;
		private bool _resetting;
		private int _lastindex = -1;
		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////


		private void RandomlySetInteractionSpot(GameObject spot)
		{
			List<Transform> interactables = new List<Transform>();

			foreach (Transform i  in spot.transform)
			{
				
				i.gameObject.SetActive(false);
				interactables.Add(i);
			}

			int randomIndex = Random.Range(0, 3);
			while (randomIndex == _lastindex)
			{
				randomIndex = Random.Range(0, 3);
			}
			_lastindex = randomIndex;
			
			spot.SetActive(true);
			interactables[randomIndex].gameObject.SetActive(true);

		}

		private void Update()
		{
			if (_playing)
			{
				if (_currentTime > 0)
				{
					_currentTime -= Time.deltaTime;
				}
				else
				{
					EndGame();
				}
			}
		}

		private void ResetSpots()
		{
			foreach (var spot in _interactableSpots)
			{
				spot.GetComponent<InteractiveSpot>().ResetInteractables();
			}
		}

		private void IncrementScore(int score)
		{
			_score += score;
			_gameUIHandler.SetScoreText(_score);

		}
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////

		public int InteractionCount
		{
			get
			{
				return _interactionsLeft;
			}
			private set
			{
				if (_interactionsLeft == value) return;
				_interactionsLeft = value;
				if (_interactionsLeft == 0 && !_resetting)
				{
					SetRotatedObject(null);

					_resetting = true;
					_audioManager.RobotEndClip();
					IncrementScore(1);
					_animationController.SetGameHeadActive(false);
					_robotRenderer.material = _robotMaterials[Random.Range(0, _robotMaterials.Length)];
					_animationController.SetGameRobot();
				}
			}
		}

		

		public void AllActiveInteractableEnable()
		{
			if (_interactables.Length == 0) return;

			foreach (Interactable i in _interactables)
			{
				if (i.gameObject.activeSelf)
				{
					i.SetInteractable(true);
				}
			}
		}

		public void DisableAllInteraction()
		{
			if (_interactables.Length == 0) return;
			foreach (Interactable i in _interactables)
			{
				i.SetInteractable(false);
			}
		}

		

		public int GetScore()
		{
			return _score;
		}

		public bool GetPlayStatus()
		{
			return _playing;
		}

		public float GetTime()
		{
			return _currentTime;
		}

		public void StartGame()
		{
			_animationController.SetMenuHeadActive(false);
			_currentTime = _gameLength;
			_score = 0;
			_gameUIHandler.SetScoreText(_score);
			_gameUIHandler.SetTimerText(_currentTime);
			var t =_gameUIHandler.FadeInOut(false);
			
			_playing = true;
			_audioManager.PlayClip("start");

		}

		public void EndGame()
		{
			DOTween.Kill("headMovement");
			_audioManager.PlayClip("win");

			_playing = false;
			_gameUIHandler.SetTimerText(0);
			_gameState.SetHighScore(_score);
			_gameState.GoToMenu();
			_animationController.SetGameHeadActive(false);
		}

		public void CompleteInteraction(GameObject caller)
		{
			InteractionCount--;
		}

		public void SetInteractionAmount(int number)
		{
			InteractionCount = number;
		}

		public void DecreamentInteractions()
		{
			InteractionCount--;
		}

		public void SetRotatedObject(GameObject toRotate)
		{
			_rotationRing.SetRotationObject(toRotate);
		}

		public void SetAllInteractionSpots()
		{
			SetInteractionAmount(4);
			foreach (var x in _interactableSpots)
			{
				RandomlySetInteractionSpot(x);
			}
			_resetting = false;
		}

		


	}
}
