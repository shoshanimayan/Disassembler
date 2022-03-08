using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using General;
using Tools;

namespace Gameplay
{
	public class DotsController : Singleton<DotsController>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] GameObject _dotsPrefab;
		[SerializeField] GameObject _dotsHolder;
		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private List<Transform> _dotsList;
		private int _currentIndex;
		private LineController _lineController { get { return LineController.Instance; } }
		private ConnectCreateController _connectCreateController { get { return ConnectCreateController.Instance; } }
		private GameStateController _gameState { get { return GameStateController.Instance; } }
		private TrackerController _trackerController { get { return TrackerController.Instance; } }

		private List<GameObject> _activeDots;
		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Awake()
		{
			_activeDots = new List<GameObject>(); 
			_currentIndex = 0;
			_dotsList = new List<Transform>();
			for(int i=0; i<100; i++)
			{
				var child = GameObject.Instantiate(_dotsPrefab,_dotsHolder.transform);
				_dotsList.Add(child.transform);
				child.GetComponent<DotView>().SetLabel((i+1).ToString());
				child.name = (i + 1).ToString();
				child.SetActive(false);
			}
		}

		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		public void ResetDots()
		{
			_activeDots.Clear();
			_currentIndex = 0;
			foreach (Transform child in _dotsList)
			{
				child.gameObject.SetActive(false);
				child.GetComponent<DotView>().ResetDot();
				_lineController.ClearLine();
			}

		}

		public void SetDot(Vector3 spot)
		{
			if (_currentIndex < _dotsList.Count)
			{
				var dot = _dotsList[_currentIndex];
				dot.position = spot;
				dot.gameObject.SetActive(true);
				_activeDots.Add(dot.gameObject);
				_currentIndex++;

				if (_gameState.State == GameState.Create  )
				{
					_connectCreateController.Add(spot);
					dot.GetComponent<DotView>().StopParticle();

				}
				else
				{
					_trackerController.SetTarget(dot);
					dot.GetComponent<DotView>().PlayParticle();
				}
				
			}
		}

		public void RemoveDot()
		{
			if (_currentIndex > 0)
			{
				_currentIndex--;
				_dotsList[_currentIndex].gameObject.SetActive(false);
				_activeDots.Remove(_dotsList[_currentIndex].gameObject);
				_connectCreateController.Removed();
			}
		}

		public void SetAllDots(Vector3[] spots)
		{
			foreach (Vector3 spot in spots)
			{
				_dotsList[_currentIndex].position = spot;
				_dotsList[_currentIndex].gameObject.SetActive(true);
				_activeDots.Add(_dotsList[_currentIndex].gameObject);
				_currentIndex++;
			}
			_lineController.AutoDraw(spots);
		}

		public void DeactiveActive()
		{
			foreach (GameObject dot in _activeDots)
			{
				dot.SetActive(false);
			}
		}

		public void ActivateActive()
		{
			foreach (GameObject dot in _activeDots)
			{
				dot.SetActive(true);
			}
		}
	}
}
