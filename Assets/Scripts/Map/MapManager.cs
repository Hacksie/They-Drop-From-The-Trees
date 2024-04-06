using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace HackedDesign
{
    public class MapManager : MonoBehaviour
    {

        [SerializeField] private NavMeshSurface surface;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private MeshCollider meshCollider;
        [SerializeField] private Transform treeParent;
        [SerializeField] private List<GameObject> trees;
        [SerializeField] private Vector2Int size = new Vector2Int(256, 256);
        [SerializeField] private int depth = 20;
        [SerializeField] private int treeCount = 500;
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
        [SerializeField] private List<string> treeTerrainTypes;
        [SerializeField] private Transform enemyParent;
        [SerializeField] private int enemyCount;
        //[SerializeField] private List<Enemy> enemyPrefabs;
        //[SerializeField] private Enemy bossPrefab;
        [SerializeField] private List<GameObject> soloPropPrefabs;
        [SerializeField] private int propCount;
        [SerializeField] private List<GameObject> propPrefabs;
        [SerializeField] private Transform propsParent;

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
            Debug.Log("Building level", this);
            CreateTerrain();
            CreateMesh();
            BuildNavMesh();
            SpawnTrees();
            SpawnEnemies();
            SpawnBoss();
            SpawnProps();
            SpawnSoloProps();
        }


        private void CreateTerrain()
        {
            terrain = new Terrain(seed, size.x, size.y, depth, meshDepthCurve, offset);
            terrain.Generate(seed, scale, octaves, persistance, lacunarity, noiseOffset, regions);
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


        private void SpawnEnemies()
        {
            for (int i = 0; i < enemyCount; i++)
            {
                var position = RandomSpawnLocation();
                var rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
                position += new Vector3(0.5f, 0, 0.5f);
                EnemyPool.Instance.SpawnRandom(position, rotation);

            }

        }

        private void SpawnBoss()
        {
            // FIXME: Handle if there are no mountain squares
            var position = RandomSpawnLocation("Mountain");
            var rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
            position += new Vector3(0.5f, 0, 0.5f);
            EnemyPool.Instance.SpawnBoss(position, rotation);
        }

        private Vector3 RandomSpawnLocation()
        {
            var location = terrain.GetRandomLocation();

            var position = new Vector3(location.x, 0, location.y);

            var t = terrain.SampleTerrain(position);
            position.y = t.meshHeight;

            return position;
        }

        private Vector3 RandomSpawnLocation(string terrainName)
        {
            var location = terrain.GetRandomLocationOfType(terrainName);
            var position = new Vector3(location.x, 0, location.y);
            var t = terrain.SampleTerrain(position);
            position.y = t.meshHeight;

            return position;
        }

        private Vector3 RandomSpawnLocation(List<string> terrainNames)
        {
            var location = terrain.GetRandomLocationOfTypes(terrainNames);
            var position = new Vector3(location.x, 0, location.y);
            var t = terrain.SampleTerrain(position);
            position.y = t.meshHeight;

            return position;
        }        

        
        private void SpawnSoloProps()
        {
            var locations = terrain.GetLocationsOfTypes(new List<string>(){"Land", "Barren", "Bush", "Rocky"});

            foreach (var prop in soloPropPrefabs)
            {
                var position2D = locations[Random.Range(0, locations.Count)];
                var position = new Vector3(position2D.x, 0, position2D.y);
                var t = terrain.SampleTerrain(position);
                position.y = t.meshHeight;
                var rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
                var go = Instantiate(prop, position, rotation, propsParent);
                PropsPool.Instance.Add(go);
            }
        }

        private void SpawnProps()
        {
            for (int i = 0; i < propCount; i++)
            {
                var position = RandomSpawnLocation(new List<string>(){"Land", "Barren", "Bush", "Rocky"});
                var rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
                //position += new Vector3(0.5f, 0, 0.5f);
                var go = Instantiate(propPrefabs[Random.Range(0, propPrefabs.Count)], position, rotation, propsParent);
                PropsPool.Instance.Add(go);
            }
        }


        private void SpawnTrees()
        {
            for (int i = 0; i < treeCount; i++)
            {
                Debug.Log("Spawn tree: " + i);
                for (int j = 0; j < 1000; j++)
                {
                    var position = RandomSpawnLocation(new List<string>(){"Land", "Barren", "Bush", "Rocky"});
                    var t = terrain.SampleTerrain(position);
                    if (Random.value < t.treeChance)
                    {
                        position.y = t.meshHeight;
                        var rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
                        position += new Vector3(0.5f, 0, 0.5f);
                        var go = Instantiate(trees[Random.Range(0, trees.Count)], position, rotation, treeParent);
                        PropsPool.Instance.Add(go);
                        break;
                    }
                }
            }
        }
    }
}