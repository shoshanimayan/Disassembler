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

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] private UnityEvent _onReachMax;

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private bool _activated;
		private HingeJoint _hinge;
		private AudioManager _audioManager { get { return AudioManager.Instance; } }
		[SerializeField] private bool _TurnOffOnEvent = true;

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void OnEnable()
		{
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<XRGrabInteractable>().enabled = true;
			_activated = false;
			_hinge = GetComponent<HingeJoint>();
			_hinge.useMotor = true;
		}

		private void DebugAngle()
		{
			print(_hinge.angle);
		}

		private void CheckIfReachedMaxAngle()
		{
			if (!_activated &&_interactable)
			{
				_activated = true;
				print(_activated);
				GetComponent<Rigidbody>().isKinematic = true;
				GetComponent<XRGrabInteractable>().enabled = false;
				_audioManager.PlayClip("blip");
				_onReachMax.Invoke();
				_interactable = false;
				if (_TurnOffOnEvent)
				{
					gameObject.SetActive(false);

				}
			}
		}



		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "leverLimit")
			{
				CheckIfReachedMaxAngle();
			}
		}

		private void OnTriggerStay(Collider other)
		{
			if (other.tag == "leverLimit")
			{
				CheckIfReachedMaxAngle();
			}
		}


		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////

		public void DestroySelf()
		{
			Destroy(gameObject);
		}

		public override void SetInteractable(bool interact)
		{
			base.SetInteractable(interact);
			_hinge.useMotor = false;

		}


	}
}
