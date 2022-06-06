using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using General;
using Gameplay;

namespace Gameplay
{
	public class RotationRing: Interactable
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
		private GameStateController _gameState { get { return GameStateController.Instance; } }

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
			_interactor = null;
			_followHand = false;
			_handOrigin = Vector3.zero;
		}

		private void GrabbedBy(SelectEnterEventArgs arg0)
		{
			_interactor = GetComponent<XRGrabInteractable>().selectingInteractor;
			_interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;
			_followHand = true;
			_handOrigin = _interactor.transform.position;

		}

		private float GetHandDirection(Vector3 CurrentHandPosition)
		{
			float direction = 0;
			float distance = Mathf.Abs(CurrentHandPosition.x - _handOrigin.x);
			if (CurrentHandPosition.x < _handOrigin.x) 
			{
				direction = 1;
			}
			if (CurrentHandPosition.x > _handOrigin.x)
			{
				direction = -1;
			}
			return direction* (distance * 5);
		}

		private void Update()
		{
			if (_followHand &&_interactable)
			{
				_objectToRotate.transform.Rotate(0, Time.deltaTime * _speed * GetHandDirection(_interactor.transform.position), 0);
			}
			if (!_interactable && _interactor)
			{
				_interactor = null;
				_followHand = false;
			}
			if (_interactor)
			{
				if (Vector3.Distance(_interactor.transform.position, transform.position) > 1f)
				{
					GrabEnd(new SelectExitEventArgs());
				}
			}

		}
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		public void SetRotationObject(GameObject obj)
		{
			_objectToRotate = obj;
		}

		
		
	}
}
