using UnityEngine;
using TMPro;
using System.Collections;

namespace HackedDesign
{
    public class DamageNumbers : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private float timeToLive = 1.0f;
        [SerializeField] private float movementDistance = 2.0f;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private Color enemyColor;
        [SerializeField] private Color playerColor;

        private Vector3 direction;
        private Vector3 start;
        private float startTime;

        public void Spawn(string text, Vector3 start, Vector3 direction, bool isPlayer)
        {
            this.startTime = Time.time;
            this.start = start;
            this.direction = direction;
            
            
            gameObject.SetActive(true);
            this.text.text = text;
            this.text.color = isPlayer ? playerColor : enemyColor;
            StartCoroutine(Die());
        }

        void Update()
        {
            if(gameObject.activeInHierarchy)
            {
                var time = (Time.time - startTime) / timeToLive;
                var position = Vector3.Lerp(start, start + (direction * movementDistance), time);
                position.y = curve.Evaluate(time);
                transform.position = position;
            }
        }

        IEnumerator Die()
        {
            yield return new WaitForSeconds(timeToLive);
            this.gameObject.SetActive(false);
        }
    }
}