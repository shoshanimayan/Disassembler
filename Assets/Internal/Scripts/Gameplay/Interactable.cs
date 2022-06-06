using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Gameplay
{
	public class Interactable: MonoBehaviour
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
				protected bool _interactable;

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////

		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		public virtual void SetInteractable(bool interact)
		{
			_interactable = interact;
		}


	

	}
}
