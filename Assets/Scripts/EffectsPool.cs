using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

namespace HackedDesign
{
    public class EffectsPool : MonoBehaviour
    {
        //[SerializeField] private GameObject bloodSplatterPrefab;
        [SerializeField] private List<GameObject> effectPrefabs;

        public static EffectsPool Instance { get; private set; }

        private List<GameObject> pool = new List<GameObject>();

        EffectsPool()
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

        public void SpawnFire(Vector3 position)
        {
            var go = Spawn("Fire", position, Quaternion.Euler(0, Random.Range(0, 359), 0));
            var fire = go.GetComponent<Fire>();
            fire.Spawn();

        }

        public void SpawnSmoke(Vector3 position)
        {
            var go = Spawn("Smoke", position, Quaternion.Euler(0, Random.Range(0, 359), 0));
            var smoke = go.GetComponent<Smoke>();
            smoke.Spawn();
        }



        public void SpawnLightning(Vector3 position)
        {
            var go = Spawn("Lightning", position, Quaternion.Euler(0, Random.Range(0, 359), 0));
            var l = go.GetComponent<Lightning>();
            l.Spawn();
        }
        public void SpawnExplosion(Vector3 position)
        {
            Spawn("Explosion", position, Quaternion.Euler(0, 90, 0));
        }

        public void SpawnBloodSplatter(Vector3 position, Quaternion rotation)
        {
            Spawn("BloodSplatter", position, rotation);
        }

        public GameObject Spawn(string name, Vector3 position, Quaternion rotation)
        {
            //var rotation = Quaternion.LookRotation(position - Game.Instance.MainCamera.transform.position);
            var fxObj = pool.FirstOrDefault(t => t.name == name && t.gameObject.activeInHierarchy == false);

            if (fxObj == null)
            {
                var prefab = effectPrefabs.FirstOrDefault(e => e.name == name);
                if (prefab == null)
                {
                    Debug.LogError("effects prefab not found", this);
                    return null;
                }
                fxObj = Instantiate(prefab, position, rotation, this.transform);
                pool.Add(fxObj);
            }
            else
            {
                fxObj.transform.position = position;
                fxObj.transform.rotation = rotation;
                fxObj.SetActive(true);
            }

            return fxObj;

            //tmp.Spawn(text, position, direction);            
        }
    }
}