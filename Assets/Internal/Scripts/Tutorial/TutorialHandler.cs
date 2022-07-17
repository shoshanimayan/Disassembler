using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using General;
using DG.Tweening;
using Gameplay;
using Lever;
using Animation;
using UnityEngine.XR.Interaction.Toolkit;

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
			if (_currentObject.GetComponent<XRGrabInteractable>())
			{
				_currentObject.GetComponent<XRGrabInteractable>().enabled = true;

			}

		}

		private void EnableTutorialRing()
		{
			EnableInteraction();
			_ring.SetTutorial(true);
			_ring.SetInteractable(true);
		}

		private void EndTutorial()
		{
			_animationController.SetTutorialHeadActive(false);
			_tutorialRing.SetActive(true);
			_gameState.SetTutorialComplete();
			_gameState.SaveGame();
			_gameState.GoToMenu();
			
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
						//setup
						DOTween.Sequence()
						.AppendCallback(() => _animationController.SetTutorialRobot())
						.AppendCallback(() => _audioManager.PlayClipWithAction("tutorial1", IncrementStep));
						break;
					case 2:
						//button
						_currentObject = _tutorialButton;
						_button.SetInteractable(false);
						DOTween.Sequence()
						.AppendCallback(() =>_tutorialBot.transform.DORotate(new Vector3(0,-90,0),.5f))
						.AppendInterval(0.05f)
						.AppendCallback(() => _audioManager.PlayClipWithAction("tutorial2",EnableInteraction));
						break;
					case 3:
						//lever
						_lever.SetInteractable(false);
						_currentObject = _tutorialLever;
						_currentObject.GetComponent<XRGrabInteractable>().enabled = false;
						DOTween.Sequence()
						.AppendCallback(() => _tutorialBot.transform.DORotate(new Vector3(0, -180, 0), .5f))
						.AppendInterval(0.05f)
						.AppendCallback(() => _audioManager.PlayClipWithAction("tutorial3", EnableInteraction));
						break;
					case 4:
						//rotator
						_currentObject = _tutorialRotator;
						_currentObject.GetComponent<XRGrabInteractable>().enabled = false;
						_rotator.SetInteractable(false);
						DOTween.Sequence()
						.AppendCallback(() => _tutorialBot.transform.DORotate(new Vector3(0,- 270, 0), .5f))
						.AppendInterval(0.05f)
						.AppendCallback(() => _audioManager.PlayClipWithAction("tutorial4", EnableInteraction));
						break;
					case 5:
						//rotate ring
						_currentObject = _tutorialRing;
						_currentObject.GetComponent<XRGrabInteractable>().enabled = false;

						DOTween.Sequence()
						.AppendInterval(0.05f)
						.AppendCallback(() => _audioManager.PlayClipWithAction("tutorial5",EnableTutorialRing ));
						break;
					case 6:
						_tutorialRing.SetActive(true);
						_ring.SetInteractable(false);
						DOTween.Sequence()
						.AppendCallback(() => _tutorialBot.transform.DORotate(new Vector3(0, -360, 0), .5f))
						.AppendInterval(0.05f)
						.AppendCallback(() => _audioManager.PlayClipWithAction("tutorial6", EndTutorial));
						//wrap up
						break;

				}
			}
		}

		public void IncrementStep()
		{
			if (_currentObject && _currentObject.GetComponent<Interactable>().IsInteractable())
			{
				_currentObject.GetComponent<Interactable>().SetInteractable(false);
				_currentObject.SetActive(false);
			}
			TutorialStep++;
		}

		public void StartTutorial()
		{
			DOTween.Sequence().SetDelay(1).AppendCallback(() => IncrementStep());

			;
		}
	}
}
