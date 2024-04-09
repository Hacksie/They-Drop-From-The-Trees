
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Agent : MonoBehaviour
    {
        [SerializeField] private UnityEngine.AI.NavMeshAgent navAgent;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Animator animator;
        [SerializeField] private bool grab;

        private Vector3 lookAt;
        private Vector3 movement;

        public bool IsEnabled { get { return navAgent.enabled; } }

        public Vector3 LookAt { get => lookAt; set => lookAt = value; }

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

        public void Stop(bool isStopped)
        {
            if (navAgent.isOnNavMesh)
            {
                navAgent.isStopped = isStopped;
            }
        }

        public void Reset()
        {
            animator.SetTrigger("Reset");
            Stop(true);
            Stop(false);
        }

        // public void Move(Vector2 movement, Vector3 direction)
        // {
        //     this.movement = Quaternion.AngleAxis(45, Vector3.up) * new Vector3(movement.x, 0, movement.y);
        //     //this.movement = movement.normalized;
        //     if (Mathf.Abs(movement.magnitude) > Mathf.Epsilon)
        //     {

        //         if(direction.magnitude > 0)
        //         {
        //             transform.forward = direction.normalized;
        //         }
                
        //         var move = new Vector3(this.movement.x, 0, this.movement.z).normalized * Time.deltaTime * Game.Instance.Settings.playerRunSpeed;
        //         //var move = (this.transform.forward * this.movement.y * Time.deltaTime * Game.Instance.Settings.playerRunSpeed) + (this.transform.right * this.movement.x * Time.deltaTime * Game.Instance.Settings.playerRunSpeed);
        //         transform.position = transform.position + move;
        //     }

        // }

        public void Pickup()
        {
            animator.SetTrigger("Pickup");
        }

        public void TakeHit()
        {
            animator.SetTrigger("Hit");
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

        public void Die()
        {
            animator.SetTrigger("Dead");
        }

        // public void LookAt(Vector3 position)
        // {
        //     this.lookAt = position;
        // }

        public void UpdateBehaviour()
        {

            Animate();
        }

        private void Animate()
        {
            animator.SetBool("isNavAgent", false);
            animator.SetFloat("moveX", this.movement.x);
            animator.SetFloat("moveY", this.movement.z);
            animator.SetFloat("Speed", navAgent.velocity.magnitude);
            animator.SetBool("Grab", grab);

        }
    }
}