using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Settings;

namespace Menu
{
	public class MenuHandler: Singleton<MenuHandler>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] TextMeshProUGUI _settingsText;
		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private SettingsController _settings { get { return SettingsController.Instance; } }

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Start()
		{
			_settingsText.text = "Change To Continuous Turn";
		}
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		public void  SwitchTurnSettingUI()
		{
			_settingsText.text= _settings.SwitchTurnProviderUI();
		}
	}
}
