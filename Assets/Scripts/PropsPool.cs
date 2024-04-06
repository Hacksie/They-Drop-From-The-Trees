using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace HackedDesign
{
    public class PropsPool : MonoBehaviour
    {
        private List<GameObject> props = new List<GameObject>();

        public static PropsPool Instance { get; private set; }

        PropsPool()
        {
            Instance = this;
        }

        public void Clear()
        {
            foreach(var go in props)
            {
                go.SetActive(false);
                Destroy(go);
            }

            props.Clear();
        }

        public void Add(GameObject gameObject)
        {
            props.Add(gameObject);
        }

        public void Remove(GameObject gameObject)
        {
            props.Remove(gameObject);
        }
    }
}