using UnityEngine;
using Cinemachine;

namespace HackedDesign
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        private float shakeTimer = 0;

        private CinemachineBasicMultiChannelPerlin perlinNoise;

        void Awake()
        {
            perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        //public void UpdateBehaviour()
        public void Update()
        {
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
                if (shakeTimer <= 0)
                {
                    shakeTimer = 0;
                    Shake(0, 0);
                }
            }
        }

        public void Shake(float intensity, float time)
        {
            perlinNoise.m_AmplitudeGain = intensity;
            shakeTimer = time;
        }        
    }
}