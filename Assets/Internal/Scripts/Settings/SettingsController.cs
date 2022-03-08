using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
namespace Settings
{
	public class SettingsController : Singleton<SettingsController>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] private ActionBasedSnapTurnProvider _snapTurn;
		[SerializeField] private ActionBasedContinuousTurnProvider _contTurn;
		[SerializeField] private ActionBasedContinuousMoveProvider _moveProvider;
		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		
		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Awake()
		{
			_moveProvider.enabled = false;

		}
		private void SwapTurnStyle()
		{
			_contTurn.enabled = !_contTurn.enabled;
			_snapTurn.enabled = !_snapTurn.enabled;

		}
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////

		public void ToggleMovementAllowed(bool active)
		{
			_moveProvider.enabled = active;
		}

		public string SwitchTurnProviderUI() {
			SwapTurnStyle();
			if (_snapTurn.enabled)
			{
				return "Change To Continuous Turn";
			}
			else
			{
				return "Change To Snap Turn";	
			}
		
		}
	}
}
