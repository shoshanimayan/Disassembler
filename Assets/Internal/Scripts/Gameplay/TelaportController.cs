using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Management;
using UnityEngine.XR;

namespace Gameplay
{
	public class TelaportController: Singleton<TelaportController>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private XRLoader _xrLoader;
		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////

		private void Start()
		{
			var xrSettings = XRGeneralSettings.Instance;
			if (xrSettings == null)
			{
				Debug.LogError($"XRGeneralSettings is null.");
				return;
			}
			var xrManager = xrSettings.Manager;
			if (xrManager == null)
			{
				Debug.LogError($"XRManagerSettings is null.");
				return;
			}

			_xrLoader = xrManager.activeLoader;
			if (_xrLoader == null)
			{
				Debug.LogError($"XRLoader is null.");
				return;
			}

		}

		private IEnumerator ResetPosition()
		{
			yield return new WaitForEndOfFrame();
			FindObjectOfType<XRRig>().transform.eulerAngles = new Vector3(0, 0, 0);
			FindObjectOfType<XRRig>().transform.position = new Vector3(0, 0, 0);
			var xrInput = _xrLoader.GetLoadedSubsystem<XRInputSubsystem>();

			if (xrInput != null)
			{
				xrInput.TrySetTrackingOriginMode(TrackingOriginModeFlags.Device);
				xrInput.TryRecenter();
			}
			else 
			{
				Debug.LogError($"XRInput: {xrInput != null}");
			}
		}
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		public void ResetRig()
		{
			StartCoroutine(ResetPosition());
		}
	}
}
