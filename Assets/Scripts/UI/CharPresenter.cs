using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace HackedDesign.UI
{
    public class CharPresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Slider healthSlider;
        [SerializeField] private UnityEngine.UI.Slider hydrationSlider;
        [SerializeField] private UnityEngine.UI.Slider sunburnSlider;
        [SerializeField] private UnityEngine.UI.Image shadeIcon;
        

        new void Awake()
        {
            base.Awake();
            if(healthSlider == null) Debug.LogError("healthSlider not set", this);
            if(hydrationSlider == null) Debug.LogError("hydrationSlider not set", this);
            if(sunburnSlider == null) Debug.LogError("sunburnSlider not set", this);
            if(shadeIcon == null) Debug.LogError("shadeIcon not set", this);
        }
  

        public override void Repaint()
        {
            healthSlider.value = GameData.Instance.health;
            hydrationSlider.value =GameData.Instance.hydration;
            sunburnSlider.value = GameData.Instance.sunburn;
            shadeIcon.gameObject.SetActive(GameData.Instance.inShade);
        }
    }
}