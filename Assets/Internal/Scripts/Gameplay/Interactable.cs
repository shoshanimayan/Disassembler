using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Gameplay
{
	public class Interactable: MonoBehaviour
	{

		

		//  PRIVATE VARIABLES         //
		
				protected bool _interactable;

	

		
		//  PUBLIC API               //
		
		public virtual void SetInteractable(bool interact)
		{
			_interactable = interact;
		}

		public virtual bool IsInteractable()
		{
			return _interactable;
		}
	

	}
}
