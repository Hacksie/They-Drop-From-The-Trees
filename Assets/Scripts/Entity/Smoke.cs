using System.Collections;
using UnityEngine;


namespace HackedDesign
{
    public class Smoke : MonoBehaviour
    {
        public void Spawn()
        {
            StartCoroutine(Timeout());
        }

        IEnumerator Timeout()
        {
            yield return new WaitForSeconds(Game.Instance.Settings.smokeTimeout);
            gameObject.SetActive(false);
        }
    }
}