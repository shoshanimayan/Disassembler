using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Gameplay
{
	public class GameHandler: Singleton<GameHandler>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] private Interactable[] _interactables=new Interactable[] { };
		[SerializeField] private RotationRing _rotationRing;

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////

		private int _score;
		private float _time;
		private int _interactionsLeft = -1;
		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Awake()
		{
			//_interactables = GameObject.FindObjectsOfType<IInteractor>();
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
		}

		private int GetScore()
		{
			return _score;
		}

		private void IncrementTime(int time)
		{
			_time += time;
		}

		public float GetTime()
		{
			return _time;
		}

		public void StartGame()
		{
			_time = 2f;
			_score = 0;
		}

		public void BasicScore(GameObject caller)
		{
			IncrementScore(100);
			InteractionCount--;
		}

		public void SetInteractionAmount(int number)
		{
			InteractionCount = number;
		}

		public void SetRotatedObject(GameObject toRotate)
		{
			_rotationRing.SetRotationObject(toRotate);
		}
	}
}
