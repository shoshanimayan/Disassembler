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
using UnityEditor;
using Animation;

namespace General
{

	[Serializable]
	class SaveData
	{
		public int HighScore;
		public string PlayerId;

		public SaveData(int score, string id)
		{
			HighScore = score;
			PlayerId = id;
		}
	}

	public enum GameState { Menu, Play,Load}


	public class GameStateController : Singleton<GameStateController>
	{

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////

		private string _playerId;
		private int _highScore=0;
		private GameState _state=GameState.Load;
		private string _key;
		private bool _subMenuOn = false;
		private  SceneLoader _sceneLoader { get { return SceneLoader.Instance; } }
		private SettingsController _settings { get { return SettingsController.Instance; } }
		private AdditionalUIController _UIController { get { return AdditionalUIController.Instance; } }
		private TelaportController _telaportController { get { return TelaportController.Instance; } }
		private AudioManager _audioManager { get { return AudioManager.Instance; } }
		private AnimationController _animationController { get { return AnimationController.Instance; } }
		private GameHandler _gameHandler { get { return GameHandler.Instance; } }



		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////

		private void Awake()
		{


		//check if playerid is saved and load it in else create
			if(!LoadGame())
			{

#if UNITY_EDITOR

				PlayerId = "unityEditor";

#else
				 PlayerId = "player";
#endif
				SaveGame(PlayerId,_highScore);
			}


		}

		private bool LoadGame()
		{

			if (File.Exists(Application.persistentDataPath + "/MySaveData.txt"))
			{

				string saveString = File.ReadAllText(Application.persistentDataPath + "/MySaveData.txt");
				SaveData data = JObject.Parse(saveString).ToObject<SaveData>();
				PlayerId = data.PlayerId;
				_highScore = data.HighScore;
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
							_settings.ToggleMovementAllowed(false);
							_UIController.ToggleLoadingUI(true);
						
							break;
						case GameState.Menu:
							_settings.ToggleMovementAllowed(true);
							_audioManager.PlayMainTheme();
							_UIController.ToggleLoadingUI(false);
							_animationController.SetMenuRobot();
							
							break;
						case GameState.Play:
							_settings.ToggleMovementAllowed(true);
							_audioManager.PlayGameTheme();
							_UIController.ToggleLoadingUI(false);
							_animationController.SetGameRobot();
							break;
						
						
					}
				}
			}
		}


		public void SaveGame(string ID,int Score)
		{
			SaveData data = new SaveData(Score,ID);
			string content = JsonConvert.SerializeObject(data);
			File.WriteAllText(Application.persistentDataPath + "/MySaveData.txt", content);

		}

		public void Play()
		{
			SetState(GameState.Load);
			SetState(GameState.Play);

		}

		public void QuitApplication()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
		}

	

		public void GoToMenu()
		{
			_gameHandler.DisableAllInteraction();
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

		private void SetHighScore(int score)
		{
			if (score > _highScore)
			{
				_highScore = score;
				SaveGame(PlayerId, _highScore);
			}
		}

		

	}
}
