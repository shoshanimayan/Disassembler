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
using Tools;

namespace General
{

	[Serializable]
	class SaveData
	{
		public string savedId;
		public Dictionary<string, Vector3[]> connectDotStorys;
	}

	public enum GameState { Menu, Play, Create,Load}


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
		private  MenuManager _menuManager { get { return MenuManager.Instance; } }
		private  HandsManager  _handsManager { get { return HandsManager.Instance; } }
		private SettingsController _settings { get { return SettingsController.Instance; } }
		private DotsController _dotsController { get { return DotsController.Instance; } }
		private AdditionalUIController _UIController { get { return AdditionalUIController.Instance; } }
		private ConnectCreateController _connectCreateController { get { return ConnectCreateController.Instance; } }
		private ConnectPlayController _connectPlayController { get { return ConnectPlayController.Instance; } }
		private TelaportController _telaportController { get { return TelaportController.Instance; } }
		private AudioManager _audioManager { get { return AudioManager.Instance; } }
		private StoryManager _storyManager { get { return StoryManager.Instance; } }
		private TrackerController _trackerController { get { return TrackerController.Instance; } }

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////

		private void Awake()
		{
			_storyManager.SetDictionary(new Dictionary<string, Vector3[]> { });
			_trackerController.ForceVisiblilty(false);


		//check if playerid is saved and load it in else create
			if(!LoadGame())
			{

#if UNITY_EDITOR
				PlayerId = "Test";

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
				SaveData data = JObject.Parse(saveString).ToObject<SaveData>();
				PlayerId = data.savedId;
				Dictionary<string, Vector3[]> TempHolder = data.connectDotStorys;
				_storyManager.SetDictionary(TempHolder);
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
					_handsManager.SetButtons(value);
					switch (value)
					{
						case GameState.Load:
							_UIController.ToggleLoadingUI(true);
							_dotsController.ResetDots();
							_settings.ToggleMovementAllowed(false);
							_trackerController.SetTarget(null);
							_trackerController.ForceVisiblilty(false);
							break;
						case GameState.Menu:
							_audioManager.StopMainTheme();
							_UIController.ToggleLoadingUI(false);
							_settings.ToggleMovementAllowed(false);
							_menuManager.EnableMenu(true, _storyManager.GetStorys());
							_trackerController.SetTarget(null);
							_trackerController.ForceVisiblilty(false);
							break;
						case GameState.Play:
							_audioManager.PlayMainTheme();
							_UIController.ToggleLoadingUI(false);
							_settings.ToggleMovementAllowed(true);
							_menuManager.EnableMenu(false);
							_connectPlayController.SetUp(_key);
							break;
						case GameState.Create:
							_audioManager.PlayMainTheme();
							_UIController.ToggleLoadingUI(false);
							_settings.ToggleMovementAllowed(true);
							_menuManager.EnableMenu(false);
							_connectCreateController.SetUp(_key);
							
						
							break;
						
					}
				}
			}
		}


		public void SaveGame(string ID)
		{
			SaveData data = new SaveData();
			data.savedId = ID;
			data.connectDotStorys =  _storyManager.GetStorys();
			string content = JsonConvert.SerializeObject(data);
			File.WriteAllText(Application.persistentDataPath + "/MySaveData.txt",content);
		}

		

		public void PlayConnect(string ConnectID)
		{
			SetState(GameState.Load);
			_key = ConnectID;
			SetState(GameState.Play);

		}

		public  void SaveAndGoToMenu(string name)
		{
			_storyManager.UpdateStorys(name, _connectCreateController.Spots);
			_connectCreateController.Clear();


			SaveGame(PlayerId);
			GoToMenu();
		}

		public void EditConnect(string ConnectID =null)
		{
			SetState(GameState.Load);
			if (ConnectID != null)
			{
				_key = ConnectID;
				SetState(GameState.Create);

			}
			else 
			{
				_key = null;
				SetState(GameState.Create);
			}
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
			if (State == GameState.Play && !_subMenuOn)
			{
				_trackerController.ForceVisiblilty(true);
			}
		}

		

	}
}
