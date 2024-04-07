using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace HackedDesign
{
    public class MapManager : MonoBehaviour
    {
        [Header("GameObjects")]
        [SerializeField] private NavMeshSurface surface;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private MeshCollider meshCollider;

        [Header("Terrain Settings")]
        [SerializeField] private Vector2Int size = new Vector2Int(256, 256);
        [SerializeField] private int depth = 20;
        [SerializeField] private float scale = 20f;
        [SerializeField] private Vector3 offset;        
        [SerializeField] private int seed = -1;
        [SerializeField] private Vector2 noiseOffset = Vector2.zero;
        [Range(1, 10)]
        [SerializeField] private int octaves = 3;
        [Range(0, 1)]
        [SerializeField] private float persistance = 0.25f;
        [Range(1, 10)]
        [SerializeField] private float lacunarity = 2;

        [SerializeField] private Renderer textureRenderer;

        [SerializeField] private TerrainType[] regions;
        [SerializeField] private AnimationCurve meshDepthCurve;

        [Header("Prop Settings")]
        [SerializeField] private int treeCount = 500;
        [SerializeField] private int waterHoleTreeCount = 20;
        [SerializeField] private int propCount;
        
        private Terrain terrain;

        public Terrain Terrain { get => terrain; set => terrain = value; }

        void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            Debug.Log("Resetting level", this);
            //PropsPool.Instance.Clear();

        }

        public void Build()
        {
            Reset();
            Debug.Log("Building level", this);
            CreateTerrain();
            CreateMesh();
            BuildNavMesh();
            SpawnTrees();
            SpawnWaterHoles();
            SpawnBoss();
            SpawnProps();
        }


        private void CreateTerrain()
        {
            terrain = new Terrain(seed, size.x, size.y, depth, meshDepthCurve, offset);
            terrain.Generate(scale, octaves, persistance, lacunarity, noiseOffset, regions);
        }

        private void CreateMesh()
        {
            var mesh = terrain.MeshData.CreateMesh();
            meshFilter.sharedMesh = mesh;
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = mesh;
            meshRenderer.sharedMaterial.mainTexture = terrain.TerrainTexture;
        }

        private void BuildNavMesh() => surface.BuildNavMesh();

        private Vector3 RandomSpawnLocation()
        {
            var location = terrain.GetRandomLocation();
            var position = terrain.TerrainPositionToWorld(location);
            position.y = terrain.TerrainMap[location.x, location.y].meshHeight;

            return position;
        }

        private Vector3 RandomSpawnLocation(string terrainName)
        {
            var location = terrain.GetRandomLocationOfType(terrainName);
            var position = terrain.TerrainPositionToWorld(location);
            position.y = terrain.TerrainMap[location.x, location.y].meshHeight;

            return position;
        }

        // private Vector3 RandomSpawnLocation(List<string> terrainNames)
        // {
        //     var location = terrain.GetRandomLocationOfTypes(terrainNames);
        //     var position = terrain.TerrainPositionToWorld(location);
        //     position.y = terrain.TerrainMap[location.x, location.y].meshHeight;

        //     return position;
        // }


        private void SpawnProps()
        {
            var locations = terrain.GetLocationsOfTypes(new List<string>() { "Land", "Barren", "Bush", "Rocky" });

            for (int i = 0; i < propCount; i++)
            {
                var location = locations[Random.Range(0, locations.Count)];
                var position = terrain.TerrainPositionToWorld(location);
                position.y = terrain.TerrainMap[location.x, location.y].meshHeight;
                position += new Vector3(0.5f, 0, 0.5f);
                EntityPool.Instance.SpawnRandomProp(position);
            }
        }


        private void SpawnTrees()
        {
            var locations = terrain.GetLocationsOfTypes(new List<string>() { "Land", "Barren", "Bush", "Rocky" });

            for (int i = 0; i < treeCount; i++)
            {
                //Debug.Log("Spawn tree: " + i);
                var location = locations[Random.Range(0, locations.Count)];
                var position = terrain.TerrainPositionToWorld(location);
                position.y = terrain.TerrainMap[location.x, location.y].meshHeight;
                position += new Vector3(0.5f, 0, 0.5f);
                EntityPool.Instance.SpawnRandomTree(position);
            }
        }

        private void SpawnWaterHoles()
        {
            var locations = terrain.GetLocationsOfTypes(new List<string>() { "Water", "Creek" });

            for (int i = 0; i < waterHoleTreeCount; i++)
            {
                Debug.Log("Spawn waterhole: " + i);

                var location = locations[Random.Range(0, locations.Count)];
                var position = terrain.TerrainPositionToWorld(location);
                position.y = terrain.TerrainMap[location.x, location.y].meshHeight;
                position += new Vector3(0.5f, 0, 0.5f);
                EntityPool.Instance.SpawnWaterhole(position);
            }
        }


        private void SpawnBoss()
        {
            // FIXME: Handle if there are no mountain squares
            var position = RandomSpawnLocation("Mountain");
            //if null, spawn at any location
            position += new Vector3(0.5f, 0, 0.5f);
            EntityPool.Instance.SpawnBossTree(position);
        }
    }
}