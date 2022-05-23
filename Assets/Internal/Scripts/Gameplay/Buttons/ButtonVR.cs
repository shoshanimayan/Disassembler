using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
namespace Gameplay
{
	public class ButtonVR: MonoBehaviour
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField]private  GameObject _button;

		[SerializeField] private UnityEvent _onPresss;
		[SerializeField] private UnityEvent _onRelease;



		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private bool _isPressed;
		private GameObject _presser;
		private AudioSource _sound;


		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Start()
		{
			_sound = GetComponent<AudioSource>();
			_isPressed = false;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!_isPressed)
			{
				_button.transform.localPosition = new Vector3(0,0.003f,0);
				_onPresss.Invoke();
				_isPressed = true;
				_sound.Play();
				_presser = other.gameObject;
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject == _presser && _isPressed)
			{
				_presser = null;
				_isPressed = false;
				_button.transform.localPosition = new Vector3(0, 0.015f, 0);
				_onRelease.Invoke();
			}
		}
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////

	}
}
