using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace HackedDesign
{
    public class EntityPool : MonoBehaviour
    {
        [Header("Trees")]
        [SerializeField] private Transform treeParent;
        [SerializeField] private List<Tree> treePrefabs;
        [SerializeField] private Tree waterholePrefab;
        [SerializeField] private Tree bossTreePrefab;
        [Header("Props")]
        [SerializeField] private Transform propParent;
        [SerializeField] private List<GameObject> propPrefabs;
        [Header("Missiles")]
        [SerializeField] private Transform missilePrefab;
        [SerializeField] private Molotov molotovPrefab;
        [SerializeField] private GameObject spearPrefab;
        [Header("Pickups")]
        [SerializeField] private List<Pickup> pickupPrefabs;


        private List<GameObject> props = new List<GameObject>();
        private List<Molotov> molotovs = new List<Molotov>();
        private List<GameObject> spears = new List<GameObject>();

        public static EntityPool Instance { get; private set; }

        EntityPool()
        {
            Instance = this;
        }

        public void Reset()
        {
           foreach (var go in props)
            {
                go.SetActive(false);
                Destroy(go);
            }

            foreach(var mol in molotovs)
            {
                mol.gameObject.SetActive(false);
                Destroy(mol.gameObject);
            }

            foreach(var spear in spears)
            {
                spear.gameObject.SetActive(false);
                Destroy(spear.gameObject);
            }

            molotovs.Clear();
            props.Clear();
        }



        private void Add(GameObject gameObject)
        {
            props.Add(gameObject);
        }

        private void Remove(GameObject gameObject)
        {
            props.Remove(gameObject);
        }

        public void SpawnMolotov(Vector3 position, Vector3 force, Quaternion rotation)
        {
            Molotov go = molotovs.FirstOrDefault(t => t.gameObject.activeInHierarchy == false);

            if (go == null)
            {
                go = Instantiate(molotovPrefab, position, rotation);
                molotovs.Add(go);
            }
            else
            {
                go.gameObject.SetActive(true);
                go.transform.SetPositionAndRotation(position, rotation);
            }

            go.Spawn();

            var rb = go.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(force, ForceMode.Impulse);
        }

        public void SpawnSpear(Vector3 position, Vector3 force, Quaternion rotation)
        {
            GameObject go = spears.FirstOrDefault(t => t.gameObject.activeInHierarchy == false);
            if (go == null)
            {
                go = Instantiate(spearPrefab, position, rotation);
                spears.Add(go);
            }
            else
            {
                go.transform.SetPositionAndRotation(position, rotation);
                go.SetActive(true);
            }

            var rb = go.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(force, ForceMode.Impulse);
        }

        public void SpawnRandomPickup(Vector3 position)
        {
            var rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
            var go = Instantiate(pickupPrefabs[Random.Range(0, pickupPrefabs.Count)], position, rotation, propParent);
            Add(go.gameObject);            
        }

        public void SpawnRandomProp(Vector3 position)
        {
            var rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
            var go = Instantiate(propPrefabs[Random.Range(0, propPrefabs.Count)], position, rotation, propParent);
            Add(go);
        }

        public void SpawnRandomTree(Vector3 position)
        {
            var rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
            var go = Instantiate(treePrefabs[Random.Range(0, treePrefabs.Count)], position, rotation, treeParent);
            Add(go.gameObject);
        }

        public void SpawnWaterhole(Vector3 position)
        {
            var go = Instantiate(waterholePrefab, position, Quaternion.identity, treeParent);
            Add(go.gameObject);
        }

        public void SpawnBossTree(Vector3 position)
        {
            var rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
            var go = Instantiate(bossTreePrefab, position, rotation, treeParent);
            Add(go.gameObject);
        }

    }
}