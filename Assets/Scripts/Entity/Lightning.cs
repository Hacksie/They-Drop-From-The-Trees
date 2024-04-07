using System.Collections;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class Lightning : MonoBehaviour
    {
        [SerializeField] private LayerMask mask;
        public void Spawn()
        {
            if(Physics.Raycast(transform.position + (Vector3.up * 20), Vector3.down, out var hit, 30, mask))
            {
                Debug.Log("l raycast hit", this);
                EffectsPool.Instance.SpawnFire(hit.point);
            }
            
            
        }


    }
}