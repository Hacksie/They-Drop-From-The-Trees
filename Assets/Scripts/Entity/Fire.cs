using System.Collections;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class Fire : MonoBehaviour
    {
        public void Spawn()
        {
            StartCoroutine(Timeout());
            StartCoroutine(Spread());
        }



        IEnumerator Timeout()
        {
            yield return new WaitForSeconds(Game.Instance.Settings.fireTimeout);
            EffectsPool.Instance.SpawnSmoke(transform.position);
            gameObject.SetActive(false);
        }

        IEnumerator Spread()
        {
            yield return new WaitForSeconds(0.5f);
            while (gameObject.activeInHierarchy)
            {
                CheckDamage();
                var settings = Game.Instance.Settings.weatherSettings.FirstOrDefault(w => w.type == GameData.Instance.currentWeather);
                if (Random.value < settings.fireSpreadChance)
                {
                    //Debug.Log("Spread");
                    var circlePos = Random.insideUnitCircle.normalized * 1.5f;
                    var position = transform.position + new Vector3(circlePos.x, 0, circlePos.y); // FIXME: Check terrain height

                    EffectsPool.Instance.SpawnFire(position);
                }
                yield return new WaitForSeconds(1);


            }
        }

        private void CheckDamage()
        {
            var settings = Game.Instance.Settings.weaponSettings.FirstOrDefault(w => w.type == WeaponType.Fire);
            var hits = Physics.OverlapSphere(this.transform.position, 1.5f);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player") || hit.CompareTag("Enemy"))
                {
                    if (Random.value > settings.missChance)
                    {
                        var damageable = hit.GetComponent<Damageable>();
                        var damage = Random.Range(settings.minDamage, settings.maxDamage);
                        damageable.Damage("Fire", WeaponType.Fire, damage);
                        DamageNumbersPool.Instance.Spawn(hit.transform.position, hit.transform.position - this.transform.position, damage.ToString(), false);
                    }
                    else
                    {
                        DamageNumbersPool.Instance.Spawn(hit.transform.position, hit.transform.position - this.transform.position, "miss", false);
                    }
                }

            }
        }
    }
}