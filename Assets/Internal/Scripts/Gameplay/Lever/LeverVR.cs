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

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void OnEnable()
		{
			_hinge = GetComponent<HingeJoint>();
			_hinge.useMotor = true;
		}

		private void DebugAngle()
		{
			print(_hinge.angle);
		}

		private void CheckIfReachedMaxAngle()
		{
			if (_hinge.angle >= _hinge.limits.max &&!_activated &&_interactable)
			{
				_activated = true;
				print(_activated);
				transform.localEulerAngles = new Vector3(_hinge.limits.max, 0,0 );
				GetComponent<Rigidbody>().isKinematic = true;
				GetComponent<XRGrabInteractable>().enabled = false;
				_audioManager.PlayClip("blip");
				_onReachMax.Invoke();
				_interactable = false;
			}
		}

		private void Update()
		{
			CheckIfReachedMaxAngle();
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
