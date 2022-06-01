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
		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private XRGrabInteractable _grabInteractor => GetComponent<XRGrabInteractable>();
		private XRBaseInteractor _interactor;

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void GrabEnd(SelectExitEventArgs arg0)
		{
		}

		private void GrabbedBy(SelectEnterEventArgs arg0)
		{
			_interactor = GetComponent<XRGrabInteractable>().selectingInteractor;
			_interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;
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
