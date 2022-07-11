using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using General;

namespace Gameplay
{
	public class ButtonVR: Interactable
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField]private  GameObject _button;

		[SerializeField] private UnityEvent _onPresss;
		[SerializeField] private UnityEvent _onRelease;
		[SerializeField] private bool _TurnOffOnEvent = true;



		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private bool _isPressed;
		private GameObject _presser;
		private AudioManager _audioManager { get { return AudioManager.Instance; } }


		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Start()
		{
			_isPressed = false;
		}

		private void OnEnable()
		{
			_button.transform.localPosition = new Vector3(0, 0.015f, 0);
			_isPressed = false;

		}

		private void OnTriggerEnter(Collider other)
		{
			if (!_isPressed && _interactable)
			{
				_button.transform.localPosition = new Vector3(0,0.003f,0);
				_onPresss.Invoke();
				_isPressed = true;
				_presser = other.gameObject;
				_audioManager.PlayClip("click");
				_interactable = false;
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject == _presser && _isPressed && !_interactable)
			{
				_presser = null;
				_isPressed = false;
				_button.transform.localPosition = new Vector3(0, 0.015f, 0);
				_audioManager.PlayClip("blip");

				_onRelease.Invoke();
				_interactable = true;
				if (_TurnOffOnEvent)
				{ gameObject.SetActive(false); }

			}
		}
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////

		

		
	}
}
