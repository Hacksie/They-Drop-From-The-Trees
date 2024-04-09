using System.Diagnostics;
//using System.Numerics;
using UnityEngine;

namespace HackedDesign
{
    public class LoadingState : IState
    {
        private PlayerController player;
        private MapManager level;

        public LoadingState(PlayerController player, MapManager level)
        {
            this.player = player;
            this.level = level;
            this.level.gameObject.SetActive(true);
        }

        public bool Playing => false;

        public void Begin()
        {
            level.gameObject.SetActive(true);
            this.level.Reset();
            this.level.Build();
            
            Physics.SyncTransforms();
            this.player.Reset();
            player.gameObject.SetActive(true);            
            var spawnPosition = Game.Instance.Settings.playerSpawn;
            var meshHeight = this.level.Terrain.SampleTerrainMeshHeight(spawnPosition);
            spawnPosition.y = meshHeight;
            
            this.player.Spawn(spawnPosition);

            Game.Instance.SetIntro();
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