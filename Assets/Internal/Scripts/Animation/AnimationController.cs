using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using General;
using Gameplay;
using Tutorial;

namespace Animation
{
	public class AnimationController: Singleton<AnimationController>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] private GameObject _blackHole;
		[SerializeField] private GameObject _menuBot;
		[SerializeField] private GameObject _gameBot;
		[SerializeField] private GameObject _tutorialBot;

		[SerializeField] private GameObject _tableSpot;

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private GameStateController _gameState { get { return GameStateController.Instance; } }
		private GameHandler _gameHandler { get { return GameHandler.Instance; } }
		private TutorialHandler _tutorialHandler { get { return TutorialHandler.Instance; } }

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Start()
		{
			_menuBot.SetActive(false);
			_gameBot.SetActive(false);
			_blackHole.SetActive(false);
			_tutorialBot.SetActive(false);
		}

		private void BlackHoleSet(bool show)
		{
			if (!show)
			{
				_blackHole.transform.DOScale(new Vector3(0, .002f, 0), 1f).OnComplete(()=>_blackHole.SetActive(false));
			}
			else 
			{
				_blackHole.SetActive(true);
				_blackHole.transform.localScale=new Vector3(0, .002f, 0);
				_blackHole.transform.DOScale(new Vector3(1, .002f, 1),1f);

			}
		}

		private void SetHeadPlacement(GameObject head)
		{

			head.SetActive(true);
			head.transform.eulerAngles = Vector3.zero;
			head.transform.position = new Vector3(_blackHole.transform.position.x, _blackHole.transform.position.y+.5f, _blackHole.transform.position.z);
			head.transform.DOMove(_tableSpot.transform.position, 2f);
		}


		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////

		public void SetMenuRobot()
		{
			DOTween.Sequence()
				.AppendInterval(0.15f)
				.AppendCallback(() => BlackHoleSet(true))
				.AppendInterval(1f)
				.AppendCallback(() => SetHeadPlacement(_menuBot))
				.AppendInterval(2f)
				.AppendCallback(() => BlackHoleSet(false)).OnComplete(()=>_gameHandler.SetRotatedObject(_menuBot)).OnComplete(() => _gameHandler.AllActiveInteractableEnable());
		}

		public void SetGameRobot()
		{
			DOTween.Sequence()
				.AppendInterval(0.15f)
				.AppendCallback(() => BlackHoleSet(true))
				.AppendInterval(1f)
				.AppendCallback(() => SetHeadPlacement(_gameBot))
				.AppendInterval(2f)
				.AppendCallback(() => BlackHoleSet(false)).OnComplete(() => _gameHandler.SetRotatedObject(_gameBot)).OnComplete(() => _gameHandler.AllActiveInteractableEnable());  
		}

		public void SetTutorialRobot()
		{
			DOTween.Sequence()
				.AppendInterval(0.15f)
				.AppendCallback(() => BlackHoleSet(true))
				.AppendInterval(1f)
				.AppendCallback(() => SetHeadPlacement(_tutorialBot))
				.AppendInterval(2f)
				.AppendCallback(() => BlackHoleSet(false)).OnComplete(() => _gameHandler.SetRotatedObject(_tutorialBot)).OnComplete(() => _tutorialHandler.IncrementStep());
		}

		public void KillRobot(GameObject obj)
		{
			obj.SetActive(false);
		}

	}
}
