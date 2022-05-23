using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Lever
{
	public class LeverVR: MonoBehaviour
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
		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Start()
		{
			_hinge = GetComponent<HingeJoint>();
			transform.localEulerAngles = new Vector3(0, 0, _hinge.limits.min);
		}

		private void CheckIfReachedMaxAngle()
		{
			if (_hinge.angle >= _hinge.limits.max &&_activated)
			{
				transform.localEulerAngles = new Vector3(0, 0, _hinge.limits.max);
				GetComponent<Rigidbody>().isKinematic = true;
				GetComponent<XRGrabInteractable>().enabled = false;
				_onReachMax.Invoke();
				


			}
		}

		private void Update()
		{
			CheckIfReachedMaxAngle();
		}
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////

	}
}
