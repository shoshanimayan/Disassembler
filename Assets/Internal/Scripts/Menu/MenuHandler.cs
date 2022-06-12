using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Settings;
using System.Threading;
using System.Threading.Tasks;
namespace Menu
{
	public class MenuHandler: Singleton<MenuHandler>
	{

		///////////////////////////////
		//  INSPECTOR VARIABLES      //
		///////////////////////////////
		[SerializeField] TextMeshProUGUI _settingsText;
		[SerializeField] private CanvasGroup _canvas;
		[SerializeField] TextMeshProUGUI _highScoreText;



		///////////////////////////////
		//  PRIVATE VARIABLES         //
		///////////////////////////////
		private SettingsController _settings { get { return SettingsController.Instance; } }

		///////////////////////////////
		//  PRIVATE METHODS           //
		///////////////////////////////
		private void Awake()
		{
			_settingsText.text = "Change To Continuous Turn";
			_canvas.alpha = 0;
		}

	
		///////////////////////////////
		//  PUBLIC API               //
		///////////////////////////////
		public void  SwitchTurnSettingUI()
		{
			_settingsText.text= _settings.SwitchTurnProviderUI();
		}

		public void SetHighScoreText(int score)
		{
			_highScoreText.text = "High Score: " + score.ToString();
		
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
