using System.Diagnostics;
//using System.Numerics;
using UnityEngine;

namespace HackedDesign
{
    public class DeadState : IState
    {
        private PlayerController player;
        private MapManager level;
        private UI.AbstractPresenter deadPanel;
        private EffectsPool effectsPool;
        private EntityPool entityPool;
        private EnemyPool enemyPool;

        public DeadState(PlayerController player, MapManager level, UI.AbstractPresenter deadPanel, EnemyPool enemyPool, EntityPool entityPool, EffectsPool effectsPool)
        {
            this.player = player;
            this.level = level;
            this.deadPanel = deadPanel;
            this.effectsPool = effectsPool;
            this.entityPool = entityPool;
            this.enemyPool = enemyPool;
            //this.level.gameObject.SetActive(true);
        }

        public bool Playing => false;

        public void Begin()
        {
            //UnityEngine.Debug.Log("Dead");
            this.deadPanel.Show();
            this.deadPanel.Repaint();
        }

        public void End()
        {
            this.deadPanel.Hide();
            this.player.Reset();
            this.enemyPool.Reset();
            this.effectsPool.Reset();
            this.entityPool.Reset();
            this.level.Reset();
            
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