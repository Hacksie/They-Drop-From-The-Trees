using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

namespace HackedDesign
{
    public class DamageNumbersPool : MonoBehaviour
    {
        [SerializeField] private DamageNumbers damagePopup;

        public static DamageNumbersPool Instance { get; private set; }

        private List<DamageNumbers> pool = new List<DamageNumbers>();

        DamageNumbersPool()
        {
            Instance = this;
        }

        public void Reset()
        {
            foreach (var obj in pool)
            {
                Destroy(obj.gameObject);
            }

            pool.Clear();
        }

        public void Spawn(Vector3 position, Vector3 direction, string text, bool isPlayer)
        {
            var rotation = Quaternion.LookRotation(position - Game.Instance.MainCamera.transform.position);
            var tmp = pool.FirstOrDefault(t => t.gameObject.activeInHierarchy == false);
            if (tmp == null)
            {
                tmp = Instantiate(damagePopup, position, rotation, this.transform);
                pool.Add(tmp);
            }
            else
            {
                tmp.transform.position = position;
                tmp.transform.rotation = rotation;
            }

            tmp.Spawn(text, position, direction, isPlayer);
        }
    }
}