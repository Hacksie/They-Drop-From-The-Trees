using System.Collections.Generic;
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

        void Awake()
        {
            if(agent == null)
            {
                agent = GetComponent<Agent>();
            }

            if(weapons == null)
            {
                weapons = GetComponent<WeaponsController>();
            }
        }

        public void Spawn()
        {

        }

        public void UpdateBehaviour()
        {
            var distanceToPlayer = DistanceToPlayer();

            if(distanceToPlayer <= attackRadius)
            {
                //Debug.Log("player attack");  
                agent.Stop();
                LookAt(Game.Instance.Player.transform.position);
                Debug.Log("Attack Player");
                weapons.Attack(Game.Instance.Player.transform.position);
            }
            else if(distanceToPlayer <= detectionRadius)
            {
                agent.MoveTo(Game.Instance.Player.transform.position);
                LookAt(Game.Instance.Player.transform.position);
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