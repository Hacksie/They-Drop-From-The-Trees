using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Tree : MonoBehaviour
    {
        [SerializeField] private List<string> enemyTypes;
        [SerializeField] private bool hasShade = false;
        //public bool canHasEnemy = true;
        [SerializeField] private bool hasEnemy = false;
        [SerializeField] private bool hasBoss = false;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(SpawnEnemies());
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (hasShade)
                {
                    SetShade();
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ClearShade();
            }
        }

        private IEnumerator SpawnEnemies()
        {
            if (hasEnemy && enemyTypes.Count > 0)
            {
                hasEnemy = false;
                var count = Random.Range(0, Game.Instance.Settings.maxEnemiesPerTree);
                for (int i = 0; i < count; i++)
                {
                    SpawnEnemy();
                    yield return new WaitForSeconds(0.5f);
                }
                
                StartCoroutine(Respawn());
            }
            else if (hasBoss)
            {
                var circlePos = Random.insideUnitCircle.normalized * 3;
                var direction = new Vector3(circlePos.x, 3, circlePos.y);
                EnemyPool.Instance.SpawnBoss(transform.position + (Vector3.up * 4), direction);
                hasBoss = false;
            }
            
        }

        private void SpawnEnemy()
        {
            var circlePos = Random.insideUnitCircle.normalized * 3;
            var direction = new Vector3(circlePos.x, 3, circlePos.y);
            EnemyPool.Instance.SpawnRandom(enemyTypes, transform.position + (Vector3.up * 4), direction);
        }

        private IEnumerator Respawn()
        {
            var respawnTimeout = Random.Range(Game.Instance.Settings.treeRespawnMinTimeout, Game.Instance.Settings.treeRespawnMaxTimeout);
            yield return new WaitForSeconds(respawnTimeout);
            hasEnemy = true;
        }

        private static void SetShade()
        {
            GameData.Instance.inShade = true;
        }

        private static void ClearShade()
        {
            GameData.Instance.inShade = false;
        }
    }
}