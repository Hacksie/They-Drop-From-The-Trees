using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace HackedDesign.UI
{
    public class DayPresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Image overcastIcon;
        [SerializeField] private UnityEngine.UI.Image dayIcon;
        [SerializeField] private UnityEngine.UI.Image rainIcon;
        [SerializeField] private UnityEngine.UI.Image stormIcon;        
        [SerializeField] private TMP_Text weatherLabel;

        public override void Repaint()
        {
            weatherLabel.text = GameData.Instance.currentWeather.ToString();
            UpdateWeatherIcon();

            
        }

        private void UpdateWeatherIcon()
        {
            var currentWeather = GameData.Instance.currentWeather;
            overcastIcon.gameObject.SetActive(currentWeather == WeatherType.Overcast);
            dayIcon.gameObject.SetActive(currentWeather == WeatherType.Sunny);
            rainIcon.gameObject.SetActive(currentWeather == WeatherType.Rain);
            stormIcon.gameObject.SetActive(currentWeather == WeatherType.Storm);

        }
   
    }
}