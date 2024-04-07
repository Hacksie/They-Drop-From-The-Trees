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
        //[SerializeField] private UnityEngine.UI.Image unshadeIcon;
        [SerializeField] private UnityEngine.UI.Image shadeIcon;
        [SerializeField] private TMP_Text spearsCounterText;
        [SerializeField] private TMP_Text bulletsCounterText;
        [SerializeField] private TMP_Text molotovCounterText;
        

        new void Awake()
        {
            base.Awake();
            if(healthSlider == null) Debug.LogError("healthSlider not set", this);
            if(hydrationSlider == null) Debug.LogError("hydrationSlider not set", this);
            if(sunburnSlider == null) Debug.LogError("sunburnSlider not set", this);
            //if(unshadeIcon == null) Debug.LogError("unshadeIcon not set", this);
            if(shadeIcon == null) Debug.LogError("shadeIcon not set", this);
            if(spearsCounterText == null) Debug.LogError("spearsCounterText not set", this);
            if(bulletsCounterText == null) Debug.LogError("bulletsCounterText not set", this);
            if(molotovCounterText == null) Debug.LogError("molotovCounterText not set", this);

        }
  

        public override void Repaint()
        {
            healthSlider.value = GameData.Instance.health;
            hydrationSlider.value =GameData.Instance.hydration;
            sunburnSlider.value = GameData.Instance.sunburn;
            //unshadeIcon.gameObject.SetActive(!GameData.Instance.inShade);
            shadeIcon.gameObject.SetActive(GameData.Instance.inShade);
            spearsCounterText.text = GameData.Instance.spears.ToString();
            bulletsCounterText.text = GameData.Instance.bullets.ToString();
            molotovCounterText.text = GameData.Instance.molotovs.ToString();
        }
    }
}