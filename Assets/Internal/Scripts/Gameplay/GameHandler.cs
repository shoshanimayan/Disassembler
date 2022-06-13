using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Threading.Tasks;
using General;
using UI;
using Animation;

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
		[SerializeField] private GameObject[] _InteractableSpots;

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private GameStateController _gameState { get { return GameStateController.Instance; } }
		private GameUIHandler _gameUIHandler { get { return GameUIHandler.Instance; } }
		private AnimationController _animationController { get { return AnimationController.Instance; } }

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
				print(i);
				i.gameObject.SetActive(false);
				interactables.Add(i);
			}

			int randomIndex = Random.Range(0, 2);
			while (randomIndex == _lastindex)
			{
				randomIndex = Random.Range(0, 2);
			}
			_lastindex = randomIndex;
			print(randomIndex);
			print(interactables[randomIndex]);
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
					_resetting = true;
					IncrementScore(1);
					_animationController.SetGameHeadActive(false);
					_animationController.SetGameRobot();
					SetRotatedObject(null);
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
		}

		public void EndGame()
		{
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
			print(toRotate);
			_rotationRing.SetRotationObject(toRotate);
		}

		public void SetAllInteractionSpots()
		{
			SetInteractionAmount(4);
			foreach (var x in _InteractableSpots)
			{
				print(x);
				RandomlySetInteractionSpot(x);
			}
			_resetting = false;
		}

		


	}
}
