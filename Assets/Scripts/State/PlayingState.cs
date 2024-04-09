using UnityEngine;

namespace HackedDesign
{
    public class PlayingState : IState
    {
        private PlayerController player;
        private DayManager dayManager;
        private EnemyPool enemyPool;
        private UI.AbstractPresenter dayPanel;
        private UI.AbstractPresenter charPanel;
        private UI.AbstractPresenter actionBarPanel;
        private UI.AbstractPresenter killPanel;

        public PlayingState(PlayerController player, DayManager dayManager, EnemyPool enemyPool, UI.AbstractPresenter dayPanel, UI.AbstractPresenter charPanel, UI.AbstractPresenter actionBarPanel, UI.AbstractPresenter killPanel)
        {
            this.player = player;
            this.dayManager = dayManager;
            this.enemyPool = enemyPool;
            this.dayPanel = dayPanel;
            this.charPanel = charPanel;
            this.actionBarPanel = actionBarPanel;
            this.killPanel = killPanel;
        }

        public bool Playing => true;

        public void Begin()
        {
            this.dayPanel.Show();
            this.charPanel.Show();
            this.actionBarPanel.Show();
            this.killPanel.Show();
        }

        public void End()
        {
            this.dayPanel.Hide();
            this.charPanel.Hide();
            this.actionBarPanel.Hide();
            this.killPanel.Hide();
        }

        public void Update()
        {
            Rehydrate();

            this.dayManager.UpdateBehaviour();
            this.player.UpdateBehaviour();
            this.enemyPool.UpdateBehaviour();
            this.dayPanel.Repaint();
            this.charPanel.Repaint();
            this.actionBarPanel.Repaint();
            this.killPanel.Repaint();

        }

        private void Rehydrate()
        {
            var location = Game.Instance.Level.Terrain.WorldPositionToTerrain(this.player.transform.position);

            var currentTile = Game.Instance.Level.Terrain.TerrainMap[location.x, location.y];

            if (currentTile.name == "Water")
            {
                GameData.Instance.hydration += Game.Instance.Settings.hydrationLossMultiplier * Time.deltaTime;
            }
        }

        public void FixedUpdate()
        {
            this.player.FixedUpdateBehaviour();
        }

        public void Menu()
        {
            Game.Instance.SetPause();
        }

        public void Select()
        {

        }


    }

}