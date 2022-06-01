using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

namespace RotationRing
{
	public class RotationRing: MonoBehaviour
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] GameObject _objectToRotate;
		[SerializeField] float _speed=1;
		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private XRGrabInteractable _grabInteractor => GetComponent<XRGrabInteractable>();
		private XRBaseInteractor _interactor;
		private bool _followHand;
		private Vector3 _handOrigin;
		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////

		private void OnEnable()
		{
			_grabInteractor.selectEntered.AddListener(GrabbedBy);
			_grabInteractor.selectExited.AddListener(GrabEnd);
		}
		private void OnDisable()
		{
			_grabInteractor.selectEntered.RemoveListener(GrabbedBy);
			_grabInteractor.selectExited.RemoveListener(GrabEnd);
		}

		private void GrabEnd(SelectExitEventArgs arg0)
		{
			_followHand = false;
			_handOrigin = Vector3.zero;
		}

		private void GrabbedBy(SelectEnterEventArgs arg0)
		{
			_interactor = GetComponent<XRGrabInteractable>().selectingInteractor;
			_interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;
			_followHand = true;
			_handOrigin = _interactor.transform.position;
			print("origin: "+ _handOrigin);

		}

		private float GetHandDirection(Vector3 CurrentHandPosition)
		{
			float direction = 0;
			if (CurrentHandPosition.x < _handOrigin.x) 
			{
				direction = 1;
			}
			if (CurrentHandPosition.x > _handOrigin.x)
			{
				direction = -1;
			}
			print(direction);
			return direction;
		}

		private void Update()
		{
			if (_followHand)
			{
				_objectToRotate.transform.Rotate(0, Time.deltaTime * _speed * GetHandDirection(_interactor.transform.position), 0);
			}

		}
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		private void SetRotationObject(GameObject obj)
		{
			_objectToRotate = obj;
		}
	}
}
