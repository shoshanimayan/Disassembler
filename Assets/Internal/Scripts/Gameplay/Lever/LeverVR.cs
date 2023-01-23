using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using General;
using Gameplay;
namespace Lever
{
	public class LeverVR: Interactable
	{

		
		//  INSPECTOR VARIABLES      //
		
		[SerializeField] private UnityEvent _onReachMax;
		[SerializeField] private bool _TurnOffOnEvent = true;

		
		//  PRIVATE VARIABLES         //
		
		private bool _activated;
		private HingeJoint _hinge;
		private AudioManager _audioManager { get { return AudioManager.Instance; } }
		private bool _setup;

		private Vector3 _rotation;
		private Vector3 _postion;

		private bool _initialized;
		
		//  PRIVATE METHODS           //
		
		private void OnEnable()
		{

			ResetLever();

		
		}

		private void Awake()
		{
			_postion = transform.localPosition;
			_rotation = transform.localEulerAngles;
			_hinge = GetComponent<HingeJoint>();
	

		}

		private void DebugAngle()
		{
			//print(_hinge.angle);
		}

		private void CheckIfReachedMaxAngle()
		{
			if (!_activated &&_interactable)
			{
				_activated = true;
				GetComponent<Rigidbody>().isKinematic = true;
				GetComponent<XRGrabInteractable>().enabled = false;
				_audioManager.PlayClip("blip");
				_onReachMax.Invoke();
				_interactable = false;
				if (_TurnOffOnEvent)
				{
					gameObject.SetActive(false);

				}
				ResetLever();
				
			}
		}


		
		private void OnCollisionEnter (Collision other)
		{
			if (other.gameObject.tag == "leverLimit" && _setup)
			{
				
					CheckIfReachedMaxAngle();
				
				
			}
			if (other.gameObject.tag == "leverTop" && !_setup && !_initialized)
			{
				_setup = true;
				_hinge.useMotor = false;
				_initialized = true;
				_rotation = transform.localEulerAngles;
				_postion = transform.localPosition;
				GetComponent<Rigidbody>().isKinematic = true;

			}
		}
		private void OnTriggerStay(Collider other)
		{
			if (other.tag == "leverLimit" && _setup)
			{

				CheckIfReachedMaxAngle();


			}
			if (other.tag == "leverTop" && !_setup)
			{
				_setup = true;
				_hinge.useMotor = false;
				GetComponent<Rigidbody>().isKinematic = true;



			}
		}


		private void ResetLever()
		{
			transform.localPosition = _postion;
			transform.localEulerAngles = _rotation;
			_setup = false;
			if (!_initialized)
			{
				_hinge.useMotor = true;
				GetComponent<Rigidbody>().isKinematic = false;
			}
			else {
				GetComponent<Rigidbody>().isKinematic = true;
				_setup = true;

			}
			GetComponent<XRGrabInteractable>().enabled = true;
			_activated = false;
			
		}

	
		//  PUBLIC API               //
		

		public void DestroySelf()
		{
			Destroy(gameObject);
		}

		

		public override void SetInteractable(bool interact)
		{
			base.SetInteractable(interact);
		

		}


	}
}
