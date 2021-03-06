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
using Menu;
using Gameplay.Height;
using Tutorial;
namespace General
{

	[Serializable]
	class SaveData
	{
		public int HighScore;
		public string PlayerId;
		public float HeightOffset;
		public bool TutorialCompleted;

		public SaveData(int score, string id, float heightOffset, bool tutorialComplete)
		{
			HighScore = score;
			PlayerId = id;
			HeightOffset= heightOffset;
			TutorialCompleted = tutorialComplete;
		}
	}

	public enum GameState { Menu, Play,Load,Tutorial}


	public class GameStateController : Singleton<GameStateController>
	{

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////

		private string _dataPath;

		private string _playerId;
		private int _highScore=0;
		private GameState _state=GameState.Load;
		private bool _subMenuOn = false;
		private bool _tutorialCompleted;
		private  SceneLoader _sceneLoader { get { return SceneLoader.Instance; } }
		private SettingsController _settings { get { return SettingsController.Instance; } }
		private AdditionalUIController _UIController { get { return AdditionalUIController.Instance; } }
		private TelaportController _telaportController { get { return TelaportController.Instance; } }
		private AudioManager _audioManager { get { return AudioManager.Instance; } }
		private AnimationController _animationController { get { return AnimationController.Instance; } }
		private GameHandler _gameHandler { get { return GameHandler.Instance; } }
		private MenuHandler _menuHandler { get { return MenuHandler.Instance; } }
		private HeightHandler _heightHandler { get { return HeightHandler.Instance; } }

		private TutorialHandler _tutorialHandler { get { return TutorialHandler.Instance; } }

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////

		private void Awake()
		{
			_dataPath= Application.persistentDataPath + "/player.fun";

			//check if playerid is saved and load it in else create
			if (!LoadGame())
			{


				PlayerId = "player";


				SaveGame();
			}


		}

		private bool LoadGame()
		{
			if (File.Exists(_dataPath))
			{

				BinaryFormatter formatter = new BinaryFormatter();
				FileStream stream = new FileStream(_dataPath, FileMode.Open);
				string saveString = formatter.Deserialize(stream) as string;
				stream.Close();

				SaveData data = JObject.Parse(saveString).ToObject<SaveData>();
				_highScore = data.HighScore;
				_heightHandler.SetInitalHeight(data.HeightOffset);
				_tutorialCompleted = data.TutorialCompleted;
				PlayerId = data.PlayerId;

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
					switch (value)
					{
						case GameState.Load:
							_settings.ToggleMovementAllowed(false);
							_UIController.ToggleLoadingUI(true);
							_heightHandler.ShowHideHandle(false);

							break;
						case GameState.Menu:
							_settings.ToggleMovementAllowed(true);
							_audioManager.PlayMainTheme();
							_UIController.ToggleLoadingUI(false);
							_animationController.SetMenuRobot();
							_heightHandler.ShowHideHandle(true);
							break;
						case GameState.Play:

							_settings.ToggleMovementAllowed(true);
							_audioManager.PlayGameTheme();
							_UIController.ToggleLoadingUI(false);

							_animationController.SetGameRobot();
							_heightHandler.ShowHideHandle(false);

							break;
						case GameState.Tutorial:
							_telaportController.ResetRig();
							_settings.ToggleMovementAllowed(true);
							_audioManager.PlayGameTheme();
							_UIController.ToggleLoadingUI(false);
							_tutorialHandler.StartTutorial();
							_heightHandler.ShowHideHandle(false);

							break;
						
					}
				}
			}
		}


		public void SaveGame()
		{
			BinaryFormatter formatter = new BinaryFormatter();

			SaveData data = new SaveData(_highScore,_playerId,_heightHandler.GetHeightChange(),_tutorialCompleted);
			string content = JsonConvert.SerializeObject(data);
		
			FileStream stream = new FileStream(_dataPath, FileMode.Create);
			formatter.Serialize(stream, content);
			stream.Close();
			//File.WriteAllText(Application.persistentDataPath + "/DisData.txt", content);

		}

		public void Play()
		{
			_gameHandler.StartGame();
			_heightHandler.SetInteractable(false);
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
			_menuHandler.SetHighScoreText(_highScore);
			SetState(GameState.Menu);
			_heightHandler.SetInteractable(true);

		}


		public bool GetSubMenuStatus()
		{
			return _subMenuOn;
		
		}

		public void ToggleSubMenu()
		{
			_subMenuOn = !_subMenuOn;
			
		}

		public void SetHighScore(int score)
		{
			if (score > _highScore)
			{
				_menuHandler.NewHighScore(true);
				_highScore = score;
				SaveGame();
			}
			else
			{
				_menuHandler.NewHighScore(false);

			}
		}

		public void SetTutorialComplete()
		{
			_tutorialCompleted = true;
			_gameHandler.SetRotatedObject(null);
			SaveGame();
		}

		public bool GetTutorialCompletedStatus()
		{
			return _tutorialCompleted;
		}

		public void GoToTutorial()
		{
			_heightHandler.SetInteractable(false);

			_gameHandler.DisableAllInteraction();
			SetState(GameState.Load);
			SetState(GameState.Tutorial);
		}

	}
}
