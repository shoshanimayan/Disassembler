using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using General;
using Tools;

namespace Gameplay
{
	public class ConnectPlayController : Singleton<ConnectPlayController>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private DotsController _dotsController { get { return DotsController.Instance; } }
		private StoryManager _storyManager { get { return StoryManager.Instance; } }
		private AudioManager _audioManager { get { return AudioManager.Instance; } }
		private TrackerController _trackerController { get { return TrackerController.Instance; } }
		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private int _index = 0;
		Vector3[] Spots;
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		public void SetUp(string story = null)
		{
			_index = 0;
			string Name = story;
			Spots = _storyManager.GetStory(story);
			_dotsController.SetDot(Spots[_index]);
			_index++;
		}

		public void NextSpot()
		{
			if (_index < Spots.Length)
			{
				_dotsController.SetDot(Spots[_index]);
				_index++;

			}
			else
			{
				_trackerController.SetTarget(null);
				_index = 0;
				_audioManager.StopMainTheme();
				_audioManager.PlayClip("CasualWin2");
			}

		}

		public bool CanClick()
		{
			return _index <= Spots.Length;

		}

		public void Clear()
		{
			_index = 0;
			Spots = null;
		}

	}
}
