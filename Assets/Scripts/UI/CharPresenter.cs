using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.UI
{
    public class CharPresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Slider healthSlider;
        [SerializeField] private UnityEngine.UI.Slider hydrationSlider;
        [SerializeField] private UnityEngine.UI.Slider sunburnSlider;
        [SerializeField] private UnityEngine.UI.Image unshadeIcon;
        [SerializeField] private UnityEngine.UI.Image shadeIcon;
  

        public override void Repaint()
        {
            healthSlider.value = GameData.Instance.health;
            hydrationSlider.value =GameData.Instance.hydration;
            sunburnSlider.value = GameData.Instance.sunburn;
            unshadeIcon.gameObject.SetActive(!GameData.Instance.inShade);
            shadeIcon.gameObject.SetActive(GameData.Instance.inShade);
        }
    }
}