using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using General;
namespace Gameplay
{
	public class HandsManager: Singleton<HandsManager>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] private HandController[] _hands;

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private DotsController _dotsController { get { return DotsController.Instance; } }
		private GameMenuController _subMenu { get { return GameMenuController.Instance; } }
		private LineController _lineController { get { return LineController.Instance; } }
		private AudioManager _audioManager { get { return AudioManager.Instance; } }
		private ConnectPlayController _playController { get { return ConnectPlayController.Instance; } }
		private ConnectCreateController _createController { get { return ConnectCreateController.Instance; } }


		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////

		private void SetNewDot(Transform t) {
	
			_dotsController.SetDot(t.position);
			_lineController.QuickDrawLine(t.position);
			_audioManager.PlayClip("Collectibles_4");
			HapticHands();

		}

		private void RemoveDot(Transform t)
		{

			_dotsController.RemoveDot();
			_lineController.RemoveLinePosition();
			HapticHands();

		}

		private void SubMenuToggle(Transform t) 
		{
			_subMenu.ActivateSubMenu();
		}

		private void ConnectDot(Transform t) 
		{
			HandController activeHand=null;
			foreach (HandController hand in _hands)
			{
				if (hand.IsActive())
				{
					activeHand = hand;
				}
			}
			if (activeHand)
			{
				if (activeHand.GetTriggeredObject() != null && _playController.CanClick() )
				{
					if (!activeHand.GetTriggeredObject().transform.GetComponent<DotView>().Selected)
					{
						_lineController.AnimatedDrawLine(activeHand.GetTriggeredObject().transform.position);
						_audioManager.PlayClip("Collectibles_4");
						activeHand.GetTriggeredObject().transform.GetComponent<DotView>().Select();
						activeHand.GetTriggeredObject().transform.GetComponent<DotView>().StopParticle();
						HapticHands();
					}
					
				}
			}
		}

		private void Awake()
		{
			foreach (HandController hand in _hands)
			{
				hand.transform.GetChild(1).gameObject.SetActive(false);
			}
		}




		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////


		public void EnableTypingHands(bool active)
		{
			foreach (HandController hand in _hands)
			{
				hand.transform.GetChild(1).gameObject.SetActive(active);
			}
		}

		public void HapticHands()
		{
			foreach (HandController hand in _hands)
			{ 
			hand.VibrateActive();
			}


		}

		public void SetButtons(GameState state)
		{
			switch (state)
			{
				case GameState.Menu:
					foreach (HandController hand in _hands)
					{
						if (hand.IsActive())
						{
							hand.gameObject.SetActive(true);
						}
						hand.ClearButtons();
						hand.PrimaryButtonMethod = null;
						hand.ActionToggleMethod = null;
					}
					break;
				case GameState.Play:
					foreach (HandController hand in _hands)
					{
						if (hand.IsActive())
						{
							hand.gameObject.SetActive(true);
						}
						hand.ClearButtons();
						hand.PrimaryButtonMethod = SubMenuToggle;
						hand.ActionToggleMethod = ConnectDot; 
					}
					break;
				case GameState.Load:
					foreach (HandController hand in _hands)
					{
						hand.gameObject.SetActive(false);
						hand.ClearButtons();
						hand.PrimaryButtonMethod = null;
						hand.ActionToggleMethod = null;
					}
					break;
				case GameState.Create:
					foreach (HandController hand in _hands)
					{
						if (hand.IsActive())
						{
							hand.gameObject.SetActive(true);
						}
						hand.ClearButtons();
						hand.ActionToggleMethod = SetNewDot;
						hand.SelectToggleMethod = RemoveDot;
						hand.PrimaryButtonMethod = SubMenuToggle;
						hand.ResetBindings();

					}
					break;
				

			}
		}
	}
}
