using UnityEngine;

namespace HackedDesign
{
    public class AgentLookAt : MonoBehaviour
    {
        [SerializeField] private Agent agent;
        [SerializeField] private Animator animator;

        void Awake()
        {
            if (agent == null)
            {
                agent = GetComponentInParent<Agent>();
            }

            if(animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            animator.SetLookAtPosition(agent.LookAt);
            animator.SetLookAtWeight(1, 0.4f, 0.7f);
        }        
    }
}