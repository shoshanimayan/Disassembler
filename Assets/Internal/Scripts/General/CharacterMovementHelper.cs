using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

namespace General
{
	public class CharacterMovementHelper: MonoBehaviour
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private XRRig _xRRig;
		private CharacterController _characterController;
		private CharacterControllerDriver _driver;
		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Start()
		{
			_xRRig = GetComponent<XRRig>();
			_characterController = GetComponent<CharacterController>();
			_driver = GetComponent<CharacterControllerDriver>();
		}

		private void Update()
		{
			UpdateCharacterController();
		}


		protected virtual void UpdateCharacterController()
		{
			if (_xRRig == null || _characterController == null)
				return;

			var height = Mathf.Clamp(_xRRig.cameraInRigSpaceHeight, _driver.minHeight, _driver.maxHeight);

			Vector3 center = _xRRig.cameraInRigSpacePos;
			center.y = height / 2f + _characterController.skinWidth;

			_characterController.height = height;
			_characterController.center = center;
		}
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////

	}
}
