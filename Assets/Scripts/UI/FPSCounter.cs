using UnityEngine;

namespace HackedDesign.UI
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Text fpsText;

        private float timer = 0;

        void Update()
        {
            if (Time.time > timer)
            {
                float fps = 1 / Time.unscaledDeltaTime;
                fpsText.text = "" + fps.ToString("N0");
                timer = Time.time + 1;
            }

        }
    }
}