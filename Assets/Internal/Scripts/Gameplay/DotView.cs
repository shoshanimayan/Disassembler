using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
namespace Gameplay
{
	public class DotView: MonoBehaviour
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] private TextMeshProUGUI _label;
		[SerializeField] private Canvas _canvas;
		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private Camera _cameraToLookAt;
		private ParticleSystem _ps;
		private Collider _collider;
		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Awake()
		{
			_collider = GetComponent<Collider>();
			Selected = false;
			_cameraToLookAt = Camera.main;
			_ps = transform.GetChild(1).GetComponent<ParticleSystem>();
		}

		private void Update()
		{
			if (_cameraToLookAt)
			{
				_canvas.transform.rotation = Quaternion.LookRotation(Vector3.Cross(Vector3.up, -Vector3.Cross(Vector3.up, _cameraToLookAt.transform.forward)), Vector3.up);
			}
		}
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		public void SetLabel(string text)
		{
			_label.text = text;
		}

		public void PlayParticle()
		{
			
				_ps.Play();
			
		}

		public void StopParticle()
		{
			_ps.Stop();
		}

		public void Select()
		{
			Selected = true;
			_collider.enabled = false;

		}

		public bool Selected { get; private set; }

		public void ResetDot()
		{
			Selected = false;
			_collider.enabled = true;		
		}
	}
}
