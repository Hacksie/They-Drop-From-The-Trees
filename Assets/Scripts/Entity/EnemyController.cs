using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    [RequireComponent(typeof(Agent), typeof(WeaponsController))]
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Agent agent;
        [SerializeField] private float detectionRadius = 0;
        [SerializeField] private float attackRadius = 0;
        [SerializeField] private List<string> preferredTerrain;
        [SerializeField] private float health = 100;
        [SerializeField] private float rotateSpeed = 60;
        [SerializeField] private WeaponsController weapons;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private Rigidbody rb; 
        
        void Awake()
        {
            if (agent == null)
            {
                agent = GetComponent<Agent>();
            }

            if (weapons == null)
            {
                weapons = GetComponent<WeaponsController>();
            }

            if(rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
        }

        public void Damage(string attacker, WeaponType type, int amount)
        {
            Debug.Log(this.name + " took :" + type.ToString() + " damage: " + amount + " from: " + attacker);
            health -= amount;
            
            if (health < 0)
            {
                health = 0;
                Die();
            }
        }

        public void Die()
        {
            EffectsPool.Instance.SpawnBloodSplatter(transform.position, Quaternion.Euler(90,0,0));
            this.gameObject.SetActive(false);
            if(GameData.Instance.killCounter.ContainsKey(this.name))
            {
                GameData.Instance.killCounter[this.name]++;
            }
            
        }

        public void Spawn()
        {
            agent.Enable(false);
            StartCoroutine(SpawnAnimation());
        }

        /// <summary>
        /// Wait 2 seconds for the spawn animation to finish before enabling the nav agent
        /// </summary>
        /// <returns></returns>
        IEnumerator SpawnAnimation()
        {
            yield return new WaitForSeconds(1.5f);
            agent.Enable(true);
            rb.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }

        public void UpdateBehaviour()
        {
            if (agent.IsEnabled)
            {
                var distanceToPlayer = DistanceToPlayer();

                if (distanceToPlayer <= attackRadius)
                {
                    agent.Stop();
                    LookAt(Game.Instance.Player.transform.position);
                    weapons.Attack(Game.Instance.Player.transform.position);
                }
                else if (distanceToPlayer <= detectionRadius)
                {
                    agent.MoveTo(Game.Instance.Player.transform.position);
                    LookAt(Game.Instance.Player.transform.position);
                }
            }
        }

        private float DistanceToPlayer()
        {
            return (Game.Instance.Player.transform.position - this.transform.position).magnitude;
        }

        public void LookAt(Vector3 target)
        {
            var rotation = Quaternion.LookRotation(target - transform.position, Vector3.up);
            var targetAngle = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetAngle, rotateSpeed * Time.deltaTime);
        }

    }
}