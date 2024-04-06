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

        public PlayingState(PlayerController player, DayManager dayManager, EnemyPool enemyPool, UI.AbstractPresenter dayPanel, UI.AbstractPresenter charPanel, UI.AbstractPresenter actionBarPanel)
        {
            this.player = player;
            this.dayManager = dayManager;
            this.enemyPool = enemyPool;
            this.dayPanel = dayPanel;
            this.charPanel = charPanel;
            this.actionBarPanel = actionBarPanel;
        }

        public bool Playing => true;

        public void Begin()
        {
            dayPanel.Show();
            charPanel.Show();
            actionBarPanel.Show();
        }

        public void End()
        {
            dayPanel.Hide();
            charPanel.Hide();
            actionBarPanel.Hide();
            //player.gameObject.SetActive(false);
        }

        public void Update()
        {
            this.dayManager.UpdateBehaviour();
            this.player.UpdateBehaviour();
            this.enemyPool.UpdateBehaviour();
            this.dayPanel.Repaint();
            this.charPanel.Repaint();
            this.actionBarPanel.Repaint();
            
        }        

        public void FixedUpdate()
        {
            this.player.FixedUpdateBehaviour();
        }

        public void Menu()
        {

        }

        public void Select()
        {

        }


    }

}