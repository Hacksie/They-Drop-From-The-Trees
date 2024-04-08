using System.Collections;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class Lightning : MonoBehaviour
    {
        [SerializeField] private float timeout = 1f;
        [SerializeField] private LayerMask mask;
        public void Spawn()
        {
            StartCoroutine(Timeout());
        }

        private IEnumerator Timeout()
        {
            yield return new WaitForSeconds(timeout);
            if (Physics.Raycast(transform.position + (Vector3.up * 20), Vector3.down, out var hit, 30, mask))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    // Arbitrary insta death
                    GameData.Instance.health = 0;
                    GameData.Instance.deathReason = DeathReason.Lightning;
                    Game.Instance.SetDead();
                }

                EffectsPool.Instance.SpawnFire(hit.point);
            }
            gameObject.SetActive(false);            
        }
    }
}