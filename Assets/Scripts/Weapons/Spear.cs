using System.Collections;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class Spear : MonoBehaviour
    {
        void Awake()
        {
            StartCoroutine(Timeout());
        }

        IEnumerator Timeout()
        {
            yield return new WaitForSeconds(Game.Instance.Settings.spearTimeout);
            gameObject.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Enemy"))
            {
                Debug.Log("hit enemy");
                var settings = Game.Instance.Settings.weaponSettings.FirstOrDefault(w => w.type == WeaponType.Spear);
                var damageable = other.GetComponent<Damageable>();
                if(damageable == null)
                {
                    Debug.LogError("hit enemy doesn't have a damageable component", this);
                    return;
                }

                if(settings == null)
                {
                    Debug.LogError("Weapon settings is null", this);
                    return;
                }
                
                var damage = Random.Range(settings.minDamage, settings.maxDamage);
                damageable.Damage("Player", WeaponType.Spear, damage);
                EffectsPool.Instance.SpawnBloodSplatter(transform.position, Random.rotation);
                DamageNumbersPool.Instance.Spawn(transform.position, transform.forward, damage.ToString(), true);                
                gameObject.SetActive(false);
            }
            else if(other.CompareTag("Terrain"))
            {
                gameObject.SetActive(false);
                DamageNumbersPool.Instance.Spawn(transform.position, transform.forward, "miss", true); 
            }
        }
    }
}