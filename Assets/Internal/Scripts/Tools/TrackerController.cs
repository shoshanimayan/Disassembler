using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using General;
using UnityEngine.UI;

namespace Tools
{
	public class TrackerController: Singleton<TrackerController>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////

		[SerializeField] private Transform _TrackerCanvas;
		[SerializeField] private Image _tracker;

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private GameStateController _gameState { get { return GameStateController.Instance; } }

		private Transform _target = null;
		private bool _visible = false;
		private Transform _camera;
		private bool _tracking {
			get { return _visible; }
			set {
				if (_visible == value) return;
				_visible = value;
				if (value) {
					_tracker.enabled = true;
				}
				else {
					if (_tracker.enabled)
					{
						_tracker.enabled = false;
					}
				}
			} 
		}

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Start()
		{
			_camera = Camera.main.transform;
		}
		private void Update()
		{
			if (_tracking && !_gameState.GetSubMenuStatus())
			{
				Track();
			}
			
		}

		private void Track()
		{
			if (_target.GetComponent<Renderer>().isVisible)
			{
				_tracker.enabled = false;

			}
			else
			{
				var direction = _camera.InverseTransformPoint(_target.position);
				var angle = -Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

				_TrackerCanvas.transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle);
				_tracker.enabled = true;
			}
			
		}

		
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		public void SetTarget(Transform t)
		{
			_target = t;
			if (_target == null)
			{
				_tracking = false;
			}
			else 
			{
				_tracking = true;
			}
		}

		public void ForceVisiblilty(bool visible)
		{
			_tracker.enabled = visible;
		}
	}
}
