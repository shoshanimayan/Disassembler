using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Gameplay
{
	public class InteractiveSpot: MonoBehaviour
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private List<Transform> _interactables;
		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Awake()
		{
			_interactables = new List<Transform>();
			foreach (Transform i in transform)
			{

				_interactables.Add(i);
			}
		}
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		public void ResetInteractables()
		{
			foreach (var i in _interactables)
			{
				i.parent = transform;
			}
		}
	}
}
