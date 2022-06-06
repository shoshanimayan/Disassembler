using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using General;
using DG.Tweening;
using Gameplay;
using Lever;

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
		[SerializeField] GameObject _tutorialRing;


		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private int _step = 0;

		private ButtonVR _button;
		private Rotator _rotator;
		private LeverVR _lever;
		private RotationRing _ring;
		private AudioManager _audioManager { get { return AudioManager.Instance; } }

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Start()
		{
			_button = _tutorialButton.GetComponent<ButtonVR>();
			_rotator = _tutorialRotator.GetComponent<Rotator>();
			_lever = _tutorialLever.GetComponent<LeverVR>();
			_ring = _tutorialRing.GetComponent<RotationRing>();
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
						//play intro and increment
					case 2:
						//button
						_tutorialButton.SetActive(true);
						DOTween.Sequence()
						.AppendInterval(0.05f)
						.AppendCallback(() => _audioManager.PlayClip("tutorial1"));
						break;
					case 3:
						//lever
						_tutorialLever.SetActive(true);
						break;
					case 4:
						//rotator
						_tutorialRotator.SetActive(true);
						break;
					case 5:
						//rotate ring
						break;
					case 6:
						//wrap up
						break;

				}
			}
		}

		public void IncrementStep()
		{ 
		
		}
	}
}
