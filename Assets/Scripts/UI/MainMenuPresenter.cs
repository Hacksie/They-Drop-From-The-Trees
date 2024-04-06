using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.UI
{
    public class MainMenuPresenter : AbstractPresenter
    {

        public override void Repaint()
        {
        }

        public void PlayAsDazzaClick()
        {
            Game.Instance.SelectCharacter(false);
            Game.Instance.SetLoading();

        }

        public void PlayAsShazzaClick()
        {
            Game.Instance.SelectCharacter(true);
            Game.Instance.SetLoading();
        }
    }
}