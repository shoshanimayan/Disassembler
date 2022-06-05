using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using General;

namespace Tutorial
{
	public class TutorialHandler: Singleton<TutorialHandler>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] GameObject _tutorialButton;
		[SerializeField] GameObject _tutorialLever;
		[SerializeField] GameObject _tutorialRotator;


		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private int _step = 0;

		private AudioManager _audioManager { get { return AudioManager.Instance; } }

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Start()
		{
			HideAllObj();
		}

		private void HideAllObj()
		{
			_tutorialButton.SetActive(false);
			_tutorialButton.SetActive(false);
			_tutorialButton.SetActive(false);

		}
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////

		public int TutorialStep
		{
			get
			{
				return _step;
			}
			private set
			{
				if (_step == value) return;
				_step = value;
				switch (_step)
				{
					case 1:
						_tutorialButton.SetActive(true);
						break;
					case 2:
						_tutorialLever.SetActive(true);
						break;
					case 3:
						_tutorialRotator.SetActive(true);
						break;
					case 4:
						//to menu
						break;

				}
			}
		}

		public void IncrementStep()
		{ 
		
		}
	}
}
