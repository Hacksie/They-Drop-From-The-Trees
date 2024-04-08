using UnityEngine;
using TMPro;
using System.Linq;
using System;

namespace HackedDesign.UI
{
    public class KillCounterPresenter : AbstractPresenter
    {
        [SerializeField] private TMP_Text text;

        public override void Repaint()
        {
            string text = "";
            foreach(var value in  GameData.Instance.killCounter)
            {
                text += value.Key + ": " + value.Value + "\n";
            }
            
            this.text.text = text;
        }


    }
}