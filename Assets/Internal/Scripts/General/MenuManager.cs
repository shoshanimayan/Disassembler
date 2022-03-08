using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEditor;
using TMPro;
using Settings;
namespace General
{
	public class MenuManager: Singleton<MenuManager>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] private GameObject _mainMenus;
		[SerializeField] private ScrollMenuController _scrollAll;
		[SerializeField] private ScrollMenuController _scrollMine;
		[SerializeField] private TextMeshProUGUI _turnButtonText;
		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private GameStateController _gameState { get { return GameStateController.Instance; } }
		private SettingsController _settings { get { return SettingsController.Instance; } }

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		public void EnableMenu(bool enabled, Dictionary<string, Vector3[]> storys=null)
		{
			 
			_scrollAll.Clear();
			_scrollMine.Clear();
			if (storys!=null)
			{
				_scrollAll.setSnapShot(storys);
				_scrollMine.setSnapShot(storys);
			}
			_mainMenus.SetActive(enabled);

		}

		public void QuitApplication()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
		}

		public void CreateNew()
		{
			_gameState.EditConnect();
		}

		

		public void  SetTurnUI()
		{

			_turnButtonText.text = _settings.SwitchTurnProviderUI();
		}

	}
}
