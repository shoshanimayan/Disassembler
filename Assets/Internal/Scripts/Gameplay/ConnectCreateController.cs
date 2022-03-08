using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using General;


namespace Gameplay
{
	public class ConnectCreateController: Singleton<ConnectCreateController>
	{

		
		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private DotsController _dotsController { get { return DotsController.Instance; } }
		private List<Vector3> _spots = new List<Vector3>();
		private GameMenuController _subMenu { get { return GameMenuController.Instance; } }
		private StoryManager _storyManager { get { return StoryManager.Instance; } }

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////

		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		public string Name = null;

		public Vector3[] Spots
		{
			get 
			{
				return _spots.ToArray();
			}
			private set 
			{
				if (_spots.ToArray() == value) return;
				_spots.AddRange(value);
				
			}
		}

		public void SetUp(string story=null )
		{
			_spots.Clear();
			if (story== null)
			{
				Spots = new Vector3[] { };
				Name = null;
			}
			else
			{
				Debug.Log(Spots.Length);
				Name = story;
				Spots = _storyManager.GetStory(story);
				_dotsController.SetAllDots(Spots);
				_subMenu.SetName(Name);

			}
		}

		public void Removed()
		{
			if (_spots!=null && _spots.Count>0)
			{
				_spots.RemoveAt(_spots.Count-1);
			}
		}

		public void Add(Vector3 spot)
		{
			_spots.Add(spot);
		}

		public void Clear()
		{
			_spots.Clear();
		}
	}
}
