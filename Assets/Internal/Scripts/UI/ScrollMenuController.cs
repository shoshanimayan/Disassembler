using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using General;
using System.Linq;

namespace UI
{
	public class ScrollMenuController: MonoBehaviour
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] private GameObject _ScrollContent;
		[SerializeField] private GameObject _ScrollButtonPrefab;
		[SerializeField] private TextMeshProUGUI _errorText;
		[SerializeField] private Vector3 _buttonRotation;
		[SerializeField] private bool _playButtons=true;
		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private GameStateController _gameState { get { return GameStateController.Instance; } }

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Start()
		{
			_errorText.enabled = false;

		}
		private void SetPlayList(string[] keys)
		{
			foreach (string key in keys)
			{
				GameObject _button = Instantiate(_ScrollButtonPrefab, new Vector3(0, 0, 0), _ScrollContent.transform.rotation, _ScrollContent.transform);
				_button.GetComponent<RectTransform>().localPosition = new Vector3(_button.GetComponent<RectTransform>().localPosition.x, _button.GetComponent<RectTransform>().localPosition.y, 0);
				_button.GetComponent<RectTransform>().localEulerAngles = _buttonRotation;
				_button.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = key;
				_button.GetComponent<Button>().onClick.AddListener(delegate { _gameState.PlayConnect(key); });

			}
		}

		private void SetEditList(string[] keys)
		{

			foreach (string key in keys)
			{
				GameObject _button = Instantiate(_ScrollButtonPrefab, new Vector3(0, 0, 0), _ScrollContent.transform.rotation, _ScrollContent.transform);
				_button.GetComponent<RectTransform>().localPosition = new Vector3(_button.GetComponent<RectTransform>().localPosition.x, _button.GetComponent<RectTransform>().localPosition.y, 0);
				_button.GetComponent<RectTransform>().localEulerAngles = _buttonRotation;
				_button.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = key;
				_button.GetComponent<Button>().onClick.AddListener(delegate { _gameState.EditConnect(key); });

			}

		}

		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		public void Clear()
		{
			if (_ScrollContent.transform.childCount > 0)
			{
				foreach (Transform child in _ScrollContent.transform)
				{
					Destroy(child.gameObject);
				}
			}
		}

		public void setSnapShot(Dictionary<string, Vector3[]> storys)
		{
			if (_playButtons)
			{
				SetPlayList(storys.Keys.ToArray());
			}
			else
			{
				SetEditList(storys.Keys.ToArray());
			}

		}

		public void SetError()
		{
			_errorText.enabled = true;
		
		}
	}
}
