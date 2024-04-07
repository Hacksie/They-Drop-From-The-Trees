using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace HackedDesign
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private List<EnemyController> enemyPrefabs = new List<EnemyController>();
        [SerializeField] private EnemyController bossPrefab;
        private List<EnemyController> enemies = new List<EnemyController>();

        public static EnemyPool Instance { get; private set; }

        EnemyPool()
        {
            Instance = this;
        }

        public void SpawnRandom(Vector3 position, Vector3 direction)
        {
            var go = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], position, Quaternion.identity, this.transform);
            go.Spawn();
            var rb = go.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddForce(direction * 2, ForceMode.Impulse);
            }   
            Add(go);
        }        

        public void SpawnRandom(List<string> names, Vector3 position, Vector3 direction)
        {
            var prefabList = enemyPrefabs.Where(e => names.Contains(e.name)).ToArray();
            if(prefabList.Length == 0)
            {
                Debug.LogError("No enemy prefabs found", this);
                return;
            }
            var go = Instantiate(prefabList[Random.Range(0, prefabList.Length)], position, Quaternion.identity, this.transform);
            go.Spawn();
            var rb = go.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddForce(direction * 2, ForceMode.Impulse);
            }            
            Add(go);
        }

        public void SpawnBoss(Vector3 position, Vector3 direction)
        {
            var go = Instantiate(bossPrefab, position, Quaternion.identity, this.transform);
            go.Spawn();
            var rb = go.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddForce(direction * 2, ForceMode.Impulse);
            }            
            Add(go);            
        }

        public void Add(EnemyController gameObject)
        {
            enemies.Add(gameObject);
        }

        public void Remove(EnemyController gameObject)
        {
            enemies.Remove(gameObject);
        }

        public void UpdateBehaviour()
        {
            foreach (var enemy in enemies.Where(e => e.gameObject.activeInHierarchy))
            {
                enemy.UpdateBehaviour();
            }
        }
    }
}