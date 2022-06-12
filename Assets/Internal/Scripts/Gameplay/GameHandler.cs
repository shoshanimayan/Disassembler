using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Threading.Tasks;
using General;
using UI;
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


		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private GameStateController _gameState { get { return GameStateController.Instance; } }
		private GameUIHandler _gameUIHandler { get { return GameUIHandler.Instance; } }

		private int _score=0;
		private float _currentTime;
		private bool _playing;
		private int _interactionsLeft = -1;
	
		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		

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
				if (_interactionsLeft == 0)
				{
					IncrementScore(1);

					//set new head
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

		private void IncrementScore(int score)
		{
			_score += score;
			_gameUIHandler.SetScoreText(_score);

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
			//var t = FadeInOut(true);
			_gameState.SetHighScore(_score);
			_gameState.GoToMenu();
		}

		public void CompleteInteraction(GameObject caller)
		{
			InteractionCount--;
		}

		public void SetInteractionAmount(int number)
		{
			InteractionCount = number;
		}

		public void SetRotatedObject(GameObject toRotate)
		{
			print(toRotate);
			_rotationRing.SetRotationObject(toRotate);
		}

		
	}
}
