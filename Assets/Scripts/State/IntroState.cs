using System.Diagnostics;
//using System.Numerics;
using UnityEngine;

namespace HackedDesign
{
    public class IntroState : IState
    {
        private UI.AbstractPresenter introPanel;

        public IntroState(UI.AbstractPresenter introPanel)
        {
            this.introPanel = introPanel;
            //this.level.gameObject.SetActive(true);
        }

        public bool Playing => false;

        public void Begin()
        {
            this.introPanel.Show();
        }

        public void End()
        {
            this.introPanel.Hide();
        }

        public void FixedUpdate()
        {

        }

        public void Menu()
        {

        }

        public void Select()
        {

        }

        public void Update()
        {
            //Game.Instance.SetLoadingProps();
            // var spawnPosition = new UnityEngine.Vector3(0, 0, 0);
            // this.player.Spawn(spawnPosition);
            // 
        }
    }

}