using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using General;
using VRKB;
using Tools;

namespace Gameplay
{
	public class GameMenuController: Singleton<GameMenuController>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] GameObject _quit;
		[SerializeField] GameObject _save;
		[SerializeField] GameObject _keyBoard;


		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private GameStateController _gameState { get { return GameStateController.Instance; } }
		private HandsManager _handsManager { get { return HandsManager.Instance; } }
		private KeyboardBehaviour _keyBoardBehavior;
		private TelaportController _telaportController { get { return TelaportController.Instance; } }
		private DotsController _dotsController { get { return DotsController.Instance; } }
		private LineController _lineController { get { return LineController.Instance; } }
		private ConnectPlayController _connectPlayController { get { return ConnectPlayController.Instance; } }
		private TrackerController _trackerController { get { return TrackerController.Instance; } }

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////

		private void DeactivateSubMenu()
		{
			if (_gameState.GetSubMenuStatus())
			{
				_gameState.ToggleSubMenu();
			}
			_keyBoardBehavior.ClearKeys();
			_quit.SetActive(false);
			_save.SetActive(false);
			_keyBoard.SetActive(false);
			_keyBoardBehavior.ClearText();

		}

		private void Start()
		{
			_keyBoardBehavior = _keyBoard.GetComponent<KeyboardBehaviour>();
			DeactivateSubMenu();
		}

		private void EnableSaveScreen()
		{
			_quit.SetActive(false);
			_save.SetActive(true);
		}

		private void StartNaming() 
		{

			_keyBoard.SetActive(true);
			_handsManager.EnableTypingHands(true);
			
		}
		

		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////

		public void FinishSaving()
		{
			if (_keyBoardBehavior.Text.Length > 0)
			{
				_handsManager.EnableTypingHands(false);
				_gameState.SaveAndGoToMenu(_keyBoardBehavior.Text);
				Resume();
			}
		}

		public void Quit()
		{
			switch (_gameState.State)
			{
				case GameState.Create:
					EnableSaveScreen();
					break;
				case GameState.Play:
					_connectPlayController.Clear();
					_gameState.GoToMenu();
					Resume();
					break;
			}
		}

		public void Resume() {
			
			_dotsController.ActivateActive();
			_lineController.EnableLine(true);
			DeactivateSubMenu();
		}

		public void ActivateSubMenu()
		{
			_dotsController.DeactiveActive();
			_lineController.EnableLine(false);
			_trackerController.ForceVisiblilty(false);
			if (!_gameState.GetSubMenuStatus())
			{
				_gameState.ToggleSubMenu();
				_quit.SetActive(true);
				_telaportController.ResetRig();
			}
		}

	

		public void Save(bool doSave) 
		{
			if (doSave)
			{
				_save.SetActive(false);
				StartNaming();

			}
			else
			{
				_gameState.GoToMenu();
				Resume();
			}

		}

		public void SetName(string name)
		{
			_keyBoardBehavior.Text = name;
		}
	}
}
