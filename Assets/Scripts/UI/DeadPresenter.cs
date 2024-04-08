using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace HackedDesign.UI
{
    public class DeadPresenter : AbstractPresenter
    {
        [SerializeField] UnityEngine.UI.Text deathText;

        public override void Repaint()
        {
            string death;
            switch (GameData.Instance.deathReason)
            {
                case DeathReason.BurntToCrisp:
                    death = "You burnt to a crisp in the Aussie sun.";
                    break;
                case DeathReason.DiedOfThirst:
                    death = "You died of thirst";
                    break;
                case DeathReason.Bitten:
                    death = "You were bitten to death by the vicious wildlife.";
                    break;
                case DeathReason.Clawed:
                    death = "You were clawed to death by the vicious wildlife.";
                    break;
                case DeathReason.BurntAlive:
                    death = "You were burnt alive by fire.";
                    break;
                case DeathReason.Killed:
                    death = "Something killed you";
                    break;
                case DeathReason.Lightning:
                    death = "You were struck my lightning and killed instantly. Such is life in the Aussie outback.";
                    break;
                case DeathReason.NotDead:
                default:
                    death = "For some reason you're not really dead.";
                break;
            }
            deathText.text = death;
        }

        public void ContinueClick()
        {
            Game.Instance.SetMainMenu();
        }
    }
}