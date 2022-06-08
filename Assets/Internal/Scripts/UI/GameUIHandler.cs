using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Threading.Tasks;
using Gameplay;

namespace UI
{
	public class GameUIHandler: Singleton<GameUIHandler>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] TextMeshProUGUI _scoreText;
		[SerializeField] TextMeshProUGUI _timerText;
		[SerializeField] private CanvasGroup _canvas;
		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////

		private GameHandler _gameHandler { get { return GameHandler.Instance; } }

		private void Awake()
		{
			_canvas.alpha = 0;

		}

		private void Update()
		{
			if (_gameHandler.GetPlayStatus())
			{
				SetTimerText(_gameHandler.GetTime());
			}
			
		}
		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private string TimeToString(float t)
		{
			float minutes = Mathf.Floor(t / 60);
			float seconds = Mathf.RoundToInt(t % 60);
			string min;
			string sec;
			if (minutes < 10)
			{
				min = "0" + minutes.ToString();
			}
			else { min = minutes.ToString(); }
			if (seconds < 10)
			{
				sec = "0" + Mathf.RoundToInt(seconds).ToString();
			}
			else { sec = Mathf.RoundToInt(seconds).ToString(); }
			return min + ":" + sec;
		}
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////

		public void SetScoreText(int score)
		{
			_scoreText.text = "Score: " + score.ToString();
		}

		public void SetTimerText(float time)
		{
			_timerText.text = "Time: " + TimeToString(time);

		}

		public async Task FadeInOut(bool fade)
		{
			if (fade)
			{
				while (_canvas.alpha > 0)
				{
					_canvas.alpha -= Time.deltaTime;

					await Task.Yield();

				}


			}
			else
			{
				while (_canvas.alpha < 1)
				{
					_canvas.alpha += Time.deltaTime;

					await Task.Yield();

				}
			}
		}
	}
}
