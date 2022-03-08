using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Gameplay;
using Settings;
using UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace General
{

	[Serializable]
	class SaveData
	{
		public int HighScore;
	
	}

	public enum GameState { Menu, Play,Load}


	public class GameStateController : Singleton<GameStateController>
	{

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////

		private string _playerId;
		private GameState _state=GameState.Load;
		private string _key;
		private bool _subMenuOn = false;
		private  SceneLoader _sceneLoader { get { return SceneLoader.Instance; } }
		private SettingsController _settings { get { return SettingsController.Instance; } }
		private AdditionalUIController _UIController { get { return AdditionalUIController.Instance; } }
		private TelaportController _telaportController { get { return TelaportController.Instance; } }
		private AudioManager _audioManager { get { return AudioManager.Instance; } }
	

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////

		private void Awake()
		{
			


		//check if playerid is saved and load it in else create
			if(!LoadGame())
			{

#if UNITY_EDITOR

#else
			//	_storyManager.CreateUser();
#endif
				SaveGame(PlayerId);
			}


		}

		private bool LoadGame()
		{
			if (File.Exists(Application.persistentDataPath + "/MySaveData.txt"))
			{
				
				string saveString = File.ReadAllText(Application.persistentDataPath + "/MySaveData.txt");
				
				Debug.Log("Game data loaded!");
				return true;
			}
			else
			{
				Debug.Log("There is no save data!");
				return false;
			}

		}

		private void SetState(GameState state)
		{
			State = state;
		}

		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		public string PlayerId
		{
			get
			{
				return _playerId;
			}
			private set
			{
				if (_playerId == value) return;
				_playerId = value;
				_sceneLoader.FirstLoad();
			}
		}

		public GameState State
		{
			get
			{
				return _state;
			}
			private set
			{
				if (_state == value) return;
				_state = value;
				if (PlayerId != null)
				{
					_telaportController.ResetRig();
					switch (value)
					{
						case GameState.Load:
							_UIController.ToggleLoadingUI(true);
						
							break;
						case GameState.Menu:
							_audioManager.StopMainTheme();
							_UIController.ToggleLoadingUI(false);
							
							break;
						case GameState.Play:
							_audioManager.PlayMainTheme();
							_UIController.ToggleLoadingUI(false);
							
							break;
						
						
					}
				}
			}
		}


		public void SaveGame(string ID)
		{
			SaveData data = new SaveData();
			data.HighScore = 0;
			
		}

		

		public void PlayConnect(string ConnectID)
		{
			SetState(GameState.Load);
			_key = ConnectID;
			SetState(GameState.Play);

		}

		

	

		public void GoToMenu()
		{
			SetState(GameState.Load);
			SetState(GameState.Menu);

		}


		public bool GetSubMenuStatus()
		{
			return _subMenuOn;
		
		}

		public void ToggleSubMenu()
		{
			_subMenuOn = !_subMenuOn;
			
		}

		

	}
}
