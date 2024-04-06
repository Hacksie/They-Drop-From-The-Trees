
using UnityEngine;


namespace HackedDesign
{
    public class WeatherController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem rain;
        [SerializeField] private ParticleSystem storm;

        public void UpdateWeather()
        {
            if(GameData.Instance.currentWeather == WeatherType.Rain && !rain.isPlaying)
            {
                rain.Play();
            }
            else if (GameData.Instance.currentWeather != WeatherType.Rain && rain.isPlaying)
            {
                rain.Stop();
            }

            if(GameData.Instance.currentWeather == WeatherType.Storm && !storm.isPlaying)
            {
                storm.Play();
            }
            else if (GameData.Instance.currentWeather != WeatherType.Storm && storm.isPlaying)
            {
                storm.Stop();
            }            
        }
    }
}
