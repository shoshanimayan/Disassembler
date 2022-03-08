using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using General;

namespace Gameplay
{
	public class HandController : MonoBehaviour
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField]  private InputActionReference _controllerActionGrip;
		[SerializeField]  private InputActionReference _controllerSelectBumper;
		[SerializeField]  private InputActionReference _controllerPrimaryButton;
		[SerializeField] private HandController _oppositeHand;
		[SerializeField] private bool _active; 
		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private GameStateController _gameState { get { return GameStateController.Instance; } }
		private bool _selected = false;
		private bool _activated = false;
		private ActionBasedController _xrc;
		private Collider _triggeredObject;
		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////

		private void OnTriggerEnter(Collider other)
		{
			_triggeredObject = other;
		}

		private void OnTriggerExit(Collider other)
		{
			_triggeredObject = null;
		}

		private void OnTriggerStay(Collider other)
		{
			_xrc.SendHapticImpulse(0.2f, 0.01f);

		}

		private void ActionToggle(InputAction.CallbackContext context)
		{
			if (_active)
			{
			
				if (ActionToggleMethod != null && !_gameState.GetSubMenuStatus() && !_activated)
				{
					_activated = true;
					ActionToggleMethod.Invoke(transform);
				}
			}
			else if (!_active && _gameState.State != GameState.Load)
			{
				ChangeHands();
			}
		}
		private void ActionUntoggle(InputAction.CallbackContext context)
		{
			if (_active)
			{
				if (ActionUntoggleMethod != null && !_gameState.GetSubMenuStatus() && _activated)
				{
					ActionUntoggleMethod.Invoke(transform);
				}
			}
			else if (!_active && _gameState.State != GameState.Load)
			{
				ChangeHands();
			}
		}

		private void SelectToggle(InputAction.CallbackContext context)
		{
			if (_active)
			{
				if (SelectToggleMethod != null && !_gameState.GetSubMenuStatus() && !_selected)
				{
					SelectToggleMethod.Invoke(transform);
					_selected = true;

				}
			}
			else if (!_active && _gameState.State != GameState.Load)
			{
				ChangeHands();
			}
		}
		private void SelectUntoggle(InputAction.CallbackContext context)
		{
			if (_active)
			{
				if (SelectUntoggleMethod != null && !_gameState.GetSubMenuStatus() && _selected)
				{
					SelectUntoggleMethod.Invoke(transform);
				}
			}
			else if (!_active && _gameState.State != GameState.Load)
			{
				ChangeHands();
			}
		}
		private void PrimaryButtonToggle(InputAction.CallbackContext context)
		{
			if (_active)
			{
				if (PrimaryButtonMethod != null && !_gameState.GetSubMenuStatus() )
				{
					PrimaryButtonMethod.Invoke(transform);
				}
			}
			else if (!_active && _gameState.State != GameState.Load)
			{
				ChangeHands();
			}
		}

		private void Start()
		{
			SelectUntoggleMethod = Unselect;
			ActionUntoggleMethod = Deactivate;

			_xrc = GetComponent<ActionBasedController>();
			_controllerActionGrip.action.started += ActionToggle;
			_controllerActionGrip.action.canceled += ActionUntoggle;
			_controllerSelectBumper.action.started += SelectToggle;
			_controllerSelectBumper.action.canceled += SelectUntoggle;
			_controllerPrimaryButton.action.started += PrimaryButtonToggle;
			gameObject.SetActive(_active);
		
		}


		private void Unselect(Transform t)
		{
			_selected = false;
			if (_activated)
			{
				_activated = false;
			}
		}

		private void Deactivate(Transform t)
		{
			_activated = false;
			if (_selected)
			{
				_selected = false;
			}
		}
		private void ChangeHands() {
			SetActive(true);
			_oppositeHand.SetActive(false);
			_activated = true;
			_selected = true;

		}

		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		public delegate void TestDelegate(Transform t); // This defines what type of method you're going to call.
		public TestDelegate ActionToggleMethod; // This is the variable holding the method you're going to call.\
		public TestDelegate ActionUntoggleMethod; // This is the variable holding the method you're going to call.
		public TestDelegate SelectToggleMethod;
		public TestDelegate SelectUntoggleMethod;
		public TestDelegate PrimaryButtonMethod;

		public void ClearButtons()
		{
			_controllerActionGrip.action.RemoveAllBindingOverrides();
			_controllerPrimaryButton.action.RemoveAllBindingOverrides();
			_controllerSelectBumper.action.RemoveAllBindingOverrides();
		}

		public Collider GetTriggeredObject()
		{
			return _triggeredObject;
		}

		public bool IsActive()
		{
			return _active;
		}

		public void SetActive(bool active)
		{
			_active = active;
			gameObject.SetActive(active);
			if(!active)
			{

				_triggeredObject = null;
			}
		}

		public void ResetBindings()
		{
			ClearButtons();
			_controllerActionGrip.action.started += ActionToggle;
			_controllerActionGrip.action.canceled += ActionUntoggle;
			_controllerSelectBumper.action.started += SelectToggle;
			_controllerSelectBumper.action.canceled += SelectUntoggle;
			_controllerPrimaryButton.action.started += PrimaryButtonToggle;
		}

		public void VibrateActive()
		{
			if (_active)
			{
				_xrc.SendHapticImpulse(0.2f, 0.2f);

			}
		}
	}
}
