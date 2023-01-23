using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Particles
{
	public class ParticleManager: Singleton<ParticleManager>
	{



		
		//  PRIVATE VARIABLES         //
		
		ParticleSystem _ps;
		
		//  PRIVATE METHODS           //
		
		private void Awake()
		{
			_ps = GetComponent<ParticleSystem>();
		}
		
		//  PUBLIC API               //
	
		public void PlayParticles()
		{
			_ps.Play();
		}
	}
}
