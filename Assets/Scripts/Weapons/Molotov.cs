using System.Collections;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class Molotov : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;
        
        void Awake()
        {

        }

        public void Spawn()
        {
            particleSystem.Clear(true);
            particleSystem.Play();
            
            StartCoroutine(Timeout());
        }


        IEnumerator Timeout()
        {
            yield return new WaitForSeconds(Game.Instance.Settings.molotovTimeout);
            particleSystem.Stop();
            gameObject.SetActive(false);
        }

        private void Explode()
        {
            EffectsPool.Instance.SpawnExplosion(transform.position);
            EffectsPool.Instance.SpawnFire(transform.position);
            var hits = Physics.OverlapSphere(transform.position, Game.Instance.Settings.molotovRadius);

            foreach(var hit in hits)
            {
                if(hit.CompareTag("Enemy") || hit.CompareTag("Player"))
                {
                    var d = hit.GetComponent<Damageable>();
                    var settings = Game.Instance.Settings.weaponSettings.FirstOrDefault(v => v.type == WeaponType.Molotov);
                    var damage = Random.Range(settings.minDamage, settings.maxDamage);
                    d.Damage("Player", WeaponType.Molotov, damage);
                    DamageNumbersPool.Instance.Spawn(hit.transform.position, hit.transform.position - transform.position, damage.ToString(), true);
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("hit enemy");
                var settings = Game.Instance.Settings.weaponSettings.FirstOrDefault(w => w.type == WeaponType.Molotov);
                var d = other.GetComponent<Damageable>();
                if (d == null)
                {
                    Debug.LogError("hit enemy doesn't have a damageable component", this);
                    return;
                }

                if (settings == null)
                {
                    Debug.LogError("Weapon settings is null", this);
                    return;
                }

                var damage = Random.Range(settings.minDamage, settings.maxDamage);
                d.Damage("Player", WeaponType.Spear, damage);
                Explode();
                
                
                particleSystem.Stop();

                gameObject.SetActive(false);
                
            }
            else if (other.CompareTag("Terrain"))
            {
                Explode();
                EffectsPool.Instance.SpawnExplosion(transform.position);
                particleSystem.Stop();
                gameObject.SetActive(false);
            }
        }
    }
}