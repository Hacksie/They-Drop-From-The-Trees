
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Agent : MonoBehaviour
    {
        [SerializeField] private UnityEngine.AI.NavMeshAgent navAgent;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Animator animator;

        private Vector3 lookAt;

        public bool IsEnabled { get { return navAgent.enabled; }}


        void Awake()
        {
            if (navAgent == null)
            {
                navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            }
        }

        void Start()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        public void Enable(bool enabled)
        {
            navAgent.enabled = enabled;
        }

        public void Stop()
        {
            navAgent.isStopped = true;
        }

        public void MoveTo(Vector3 position)
        {
            navAgent.SetDestination(position);
        }

        public void TeleportTo(Vector3 position)
        {
            Debug.Log("Teleport " + position);
            navAgent.enabled = false;
            rb.MovePosition(position);
            navAgent.Warp(position);
            navAgent.enabled = true;
        }

        public void LookAt(Vector3 position)
        {
            this.lookAt = position;
        }

        public void UpdateBehaviour()
        {

            Animate();
        }

        private void Animate()
        {
            animator.SetFloat("Speed", navAgent.velocity.magnitude);

        }

        private void OnAnimatorIK(int layerIndex)
        {
            animator.SetLookAtPosition(lookAt);
            animator.SetLookAtWeight(1, 0.4f, 0.7f);
        }

    }

    public enum WeaponType
    {
        Punch,
        Knife,
        Spear,
        Rifle,
        Molotov,
        Bite,
        Claw
    }
}