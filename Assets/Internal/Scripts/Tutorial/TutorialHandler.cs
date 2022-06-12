using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using General;
using DG.Tweening;
using Gameplay;
using Lever;
using Animation;

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
		[SerializeField] GameObject _tutorialBot;

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private int _step = 0;

		private ButtonVR _button;
		private Rotator _rotator;
		private LeverVR _lever;
		private RotationRing _ring;

		private GameObject _currentObject;
		private AudioManager _audioManager { get { return AudioManager.Instance; } }
		private GameStateController _gameState { get { return GameStateController.Instance; } }
		private AnimationController _animationController { get { return AnimationController.Instance; } }

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
		//  PRIVATE METHODS           //
		///////////////////////////////

		private void EnableInteraction()
		{
			_currentObject.GetComponent<Interactable>().SetInteractable(true);
		}

		private void EnableTutorialRing()
		{
			_ring.GetComponent<RotationRing>().SetTutorial(true);
			_ring.GetComponent<RotationRing>().SetInteractable(true);
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
						DOTween.Sequence()
						.AppendCallback(() => _animationController.SetTutorialRobot())
						.AppendCallback(() => _audioManager.PlayClipWithAction("tutorial2", IncrementStep));
						break;
					case 2:
						//button
						_tutorialButton.SetActive(true);
						_currentObject = _tutorialButton;
						DOTween.Sequence()
						.AppendCallback(() =>_tutorialBot.transform.DORotate(new Vector3(0,90,0),.5f))
						.AppendInterval(0.05f)
						.AppendCallback(() => _audioManager.PlayClipWithAction("tutorial2",EnableInteraction));
						break;
					case 3:
						//lever

						_tutorialLever.SetActive(true);
						_currentObject = _tutorialLever;
						DOTween.Sequence()
						.AppendCallback(() => _tutorialBot.transform.DORotate(new Vector3(0, 180, 0), .5f))
						.AppendInterval(0.05f)
						.AppendCallback(() => _audioManager.PlayClipWithAction("tutorial3", EnableInteraction));
						break;
					case 4:
						//rotator
						_tutorialRotator.SetActive(true);
						_currentObject = _tutorialRotator;
						DOTween.Sequence()
						.AppendCallback(() => _tutorialBot.transform.DORotate(new Vector3(0, 270, 0), .5f))
						.AppendInterval(0.05f)
						.AppendCallback(() => _audioManager.PlayClipWithAction("tutorial4", EnableInteraction));
						break;
					case 5:
						//rotate ring
						_currentObject = null;
						DOTween.Sequence()
						.AppendInterval(0.05f)
						.AppendCallback(() => _audioManager.PlayClipWithAction("tutorial4",EnableTutorialRing ));
						break;
					case 6:
						DOTween.Sequence()
						.AppendCallback(() => _tutorialBot.transform.DORotate(new Vector3(0, 360, 0), .5f))
						.AppendInterval(0.05f)
						.AppendCallback(() => _audioManager.PlayClipWithAction("tutorial6", _gameState.GoToMenu));
						//wrap up
						break;

				}
			}
		}

		public void IncrementStep()
		{
			if (_currentObject)
			{
				_currentObject.GetComponent<Interactable>().SetInteractable(false);
				_currentObject.SetActive(false);
			}
			TutorialStep++;
		}
	}
}
