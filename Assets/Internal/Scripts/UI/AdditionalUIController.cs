using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace UI
{
	public class AdditionalUIController: Singleton<AdditionalUIController>
	{

		//  INSPECTOR VARIABLES      //
		
		[SerializeField] private GameObject _loadingCanvas;
		[SerializeField] private GameObject _loadingSphere;
	
		//  PUBLIC API               //
		
		public void ToggleLoadingUI(bool active) 
		{
			_loadingCanvas.SetActive(active);
			if (_loadingSphere.activeSelf)
			{
				_loadingSphere.SetActive(active);
			}
		}
	}
}
