using System.Diagnostics;
//using System.Numerics;
using UnityEngine;

namespace HackedDesign
{
    public class MainMenuState : IState
    {
        UI.AbstractPresenter mainMenuPanel;
        private Camera menuCamera;
        private Camera mainCamera;        

        public MainMenuState(Camera mainCamera, Camera menuCamera, UI.AbstractPresenter mainMenuPanel)
        {
            this.menuCamera = menuCamera;
            this.mainCamera = mainCamera;
            this.mainMenuPanel = mainMenuPanel;
        }

        public bool Playing => false;

        public void Begin()
        {
            Game.Instance.Reset();
            this.mainMenuPanel.Show();
            mainCamera.gameObject.SetActive(false);
            menuCamera.gameObject.SetActive(true);            

                
        }

        public void End()
        {
            mainCamera.gameObject.SetActive(true);
            menuCamera.gameObject.SetActive(false);
            this.mainMenuPanel.Hide();
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
            this.mainMenuPanel.Repaint();
 
        }
    }

}