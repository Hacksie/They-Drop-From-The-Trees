using System.Diagnostics;
//using System.Numerics;
using UnityEngine;

namespace HackedDesign
{
    public class PauseState : IState
    {
        private UI.AbstractPresenter pausePanel;

        public PauseState(UI.AbstractPresenter pausePanel)
        {
            this.pausePanel = pausePanel;
            //this.level.gameObject.SetActive(true);
        }

        public bool Playing => false;

        public void Begin()
        {
            this.pausePanel.Show();
        }

        public void End()
        {
            this.pausePanel.Hide();
        }

        public void FixedUpdate()
        {

        }

        public void Menu()
        {
            Game.Instance.SetPlaying();
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