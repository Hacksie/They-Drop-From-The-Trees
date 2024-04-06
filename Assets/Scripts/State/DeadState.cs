using System.Diagnostics;
//using System.Numerics;
using UnityEngine;

namespace HackedDesign
{
    public class DeadState : IState
    {
        private PlayerController player;
        private MapManager level;

        public DeadState(PlayerController player, MapManager level)
        {
            this.player = player;
            this.level = level;
            //this.level.gameObject.SetActive(true);
        }

        public bool Playing => false;

        public void Begin()
        {
            UnityEngine.Debug.Log("Dead");
        }

        public void End()
        {

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