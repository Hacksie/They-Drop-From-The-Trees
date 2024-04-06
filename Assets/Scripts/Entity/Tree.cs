using UnityEngine;

namespace HackedDesign
{
    public class Tree : MonoBehaviour
    {
        [SerializeField] private bool hasShade = false;
        public bool hasEnemy = false;

        void Awake()
        {
            hasEnemy = Random.value < Game.Instance.Settings.treeEnemyChance;
        }
        void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Trigger enter " + other.name);
            if (other.CompareTag("Player"))
            {
                GameData.Instance.inShade = true;

                Debug.Log("Shade: " + GameData.Instance.inShade);
                
                if (Random.value < Game.Instance.Settings.treeEnemyChance)
                {
                    Debug.Log("Tree spawns enemy");
                    var circlePos = Random.insideUnitCircle.normalized * 3;
                    var position = transform.position + new Vector3(circlePos.x, 0, circlePos.y);
                    EnemyPool.Instance.SpawnRandom(position,Quaternion.identity);

                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            GameData.Instance.inShade = false;
            Debug.Log("Shade: " + GameData.Instance.inShade);
        }
    }
}