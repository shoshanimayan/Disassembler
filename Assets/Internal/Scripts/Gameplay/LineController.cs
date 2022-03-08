using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Gameplay
{
	public class LineController : Singleton<LineController>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] private LineRenderer _line;

		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private int _index=0;
		private readonly int _maxSpots = 100;
		private ConnectPlayController _PlayController { get { return ConnectPlayController.Instance; } }

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Start()
		{
			_index = 0;
			_line.enabled = false;
		}

		private void StartUpLine()
		{
			_line.enabled = true;
			_line.positionCount = 100;
			_index = 0;
		}

		private void EndLine()
		{
			_line.enabled = false;
			_line.positionCount = 0;
			_index = 0;
		}

		private void UpdatePositions(Vector3 pos)
		{
			if (!_line.enabled)
			{
				StartUpLine();
			}
			for (int i = _index; i < _line.positionCount; i++)
			{
				_line.SetPosition(i, pos);
			}
		}

		private IEnumerator DrawLine(Vector3 EndPos)
		{
			float t = 0;
			float time = 1;
			Vector3 origin = _line.GetPosition(_index);
			Vector3 newpos;
			for (; t < time; t += Time.deltaTime)
			{
				newpos = Vector3.Lerp(origin, EndPos, t );
				UpdatePositions(newpos);
				yield return null;
			}
			_index++;
			_PlayController.NextSpot();
			UpdatePositions(EndPos);
		}

		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////

		//draw lines for all spots
		public void AutoDraw(Vector3[] spots)
		{
			for (int i = 0; i < spots.Length; i++)
			{
				UpdatePositions(spots[i]);
				_index++;
			}
		}

		//quickly draw line
		public void QuickDrawLine(Vector3 spot)
		{
			if (_index < _maxSpots)
			{
				UpdatePositions(spot);
				_index++;
			}
		}


		//animate the line being drawn
		public void AnimatedDrawLine(Vector3 spot)
		{
			if (_index > 0)
			{
				StartCoroutine(DrawLine(spot));
			}
			else
			{
				UpdatePositions(spot);
				_index++;
				_PlayController.NextSpot();

			}
		}

		public void RemoveLinePosition()
		{
			if (_index > 0)
			{
				_index --;
				if (_index - 1 > 0)
				{
					UpdatePositions(_line.GetPosition(_index - 1));
				}
				else
				{
					EndLine();
				}
			}
		}

		public void ClearLine()
		{
			_line.SetPositions(new Vector3[] { });
			_line.positionCount = 0;
			_line.enabled = false;
			_index = 0;
		}

		public void EnableLine(bool active)
		{
			_line.enabled = active;
		}


	}
}
