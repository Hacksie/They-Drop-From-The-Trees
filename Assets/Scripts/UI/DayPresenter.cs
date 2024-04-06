using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace HackedDesign.UI
{
    public class DayPresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Image dawnIcon;
        [SerializeField] private UnityEngine.UI.Image dayIcon;
        [SerializeField] private UnityEngine.UI.Image duskIcon;
        [SerializeField] private UnityEngine.UI.Image nightIcon;
        [SerializeField] private TMP_Text weatherLabel;
        [SerializeField] private TMP_Text dayLabel;
        [SerializeField] private TMP_Text timeLabel;

        public override void Repaint()
        {
            weatherLabel.text = GameData.Instance.currentWeather.ToString();
            TimeSpan time = TimeSpan.FromSeconds(GameData.Instance.currentTime);
            dayLabel.text = "Day " + GameData.Instance.currentDay.ToString();
            timeLabel.text = time.ToString("hh':'mm", System.Globalization.CultureInfo.InvariantCulture);            
            UpdateDayPhaseIcon();
        }

        private void UpdateDayPhaseIcon()
        {
            var phase = DayManager.GetDayPhase(GameData.Instance.currentTime, Game.Instance.Settings);
            dawnIcon.gameObject.SetActive(phase == DayPhase.Dawn);
            dayIcon.gameObject.SetActive(phase == DayPhase.Day);
            duskIcon.gameObject.SetActive(phase == DayPhase.Dusk);
            nightIcon.gameObject.SetActive(phase == DayPhase.Night);

        }
    
    }
}