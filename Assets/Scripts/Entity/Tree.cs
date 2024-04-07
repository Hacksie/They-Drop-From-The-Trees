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
                SpawnEnemy();
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

        private void SpawnEnemy()
        {
            if (hasEnemy && enemyTypes.Count > 0)
            {
                var count = Random.Range(0, Game.Instance.Settings.maxEnemiesPerTree);
                for (int i = 0; i < count; i++)
                {
                    var circlePos = Random.insideUnitCircle.normalized * 4;
                    var direction = new Vector3(circlePos.x, 3, circlePos.y);
                    EnemyPool.Instance.SpawnRandom(enemyTypes, transform.position + (Vector3.up * 3), direction);
                }

                hasEnemy = false;
            }
            else if (hasBoss)
            {
                var circlePos = Random.insideUnitCircle.normalized * 4;
                var direction = new Vector3(circlePos.x, 3, circlePos.y);
                EnemyPool.Instance.SpawnBoss(transform.position + (Vector3.up * 3), direction);
                hasBoss = false;
            }
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