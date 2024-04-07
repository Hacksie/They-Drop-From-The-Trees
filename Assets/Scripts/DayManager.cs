using System.Collections;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class DayManager : MonoBehaviour
    {
        public const int secondsPerDay = 86400;
        [SerializeField] private Light dawnLight;
        [SerializeField] private Light dayLight;
        [SerializeField] private Light duskLight;
        [SerializeField] private Light nightLight;
        [SerializeField] private Light stormLight;
        [SerializeField] private Light rainLight;
        [SerializeField] private Light overcastLight;

        public static DayManager Instance { get; private set; }

        //private DayPhase currentDayPhase = DayPhase.Day;

        private float currentWeatherTime = 0;

        DayManager()
        {
            Instance = this;
        }


        public void UpdateBehaviour()
        {
            currentWeatherTime += Time.deltaTime;
            if (currentWeatherTime > Game.Instance.Settings.weatherChangeRate)
            {
                Debug.Log("Update weather");
                currentWeatherTime = 0;
                var nextWeather = Random.Range(0, 4);
                GameData.Instance.currentWeather = (WeatherType)nextWeather;
                
                if (GameData.Instance.currentWeather == WeatherType.Storm)
                {
                    StartCoroutine(UpdateLightning());
                }
            }


            UpdateSunburn();
            UpdateHydration();
            UpdateLight();
            UpdateLightning();
        }

        private IEnumerator UpdateLightning()
        {

            while(GameData.Instance.currentWeather == WeatherType.Storm)
            {                
                var location = Game.Instance.Level.Terrain.GetRandomLocation();
                var position = Game.Instance.Level.Terrain.TerrainPositionToWorld(location);
                EffectsPool.Instance.SpawnLightning(position);
                yield return new WaitForSeconds(1);
            }
        }

        private void UpdateSunburn()
        {
            var weatherSettings = Game.Instance.Settings.weatherSettings.FirstOrDefault(w => w.type == GameData.Instance.currentWeather);

            if (weatherSettings == null)
            {
                Debug.LogError("No weather settings set for " + GameData.Instance.currentWeather.ToString(), this);
            }

            GameData.Instance.sunburn += Time.deltaTime * weatherSettings.sunburnRate;
            GameData.Instance.sunburn = Mathf.Clamp(GameData.Instance.sunburn, 0, Game.Instance.Settings.maxSunburn);

            if (GameData.Instance.sunburn >= Game.Instance.Settings.maxSunburn)
            {
                Game.Instance.Die(DeathReason.BurntToCrisp);
            }
        }

        private void UpdateHydration()
        {
            GameData.Instance.hydration -= Time.deltaTime * Game.Instance.Settings.hydrationLossPerSecondNormal * (1 + (GameData.Instance.sunburn / 100));

            GameData.Instance.hydration = Mathf.Clamp(GameData.Instance.hydration, 0, Game.Instance.Settings.maxHydration);
            if (GameData.Instance.hydration <= 0)
            {
                Game.Instance.Die(DeathReason.DiedOfThirst);
            }
        }


        private void UpdateLight()
        {
            var currentWeather = GameData.Instance.currentWeather;
            overcastLight.gameObject.SetActive(currentWeather == WeatherType.Overcast);
            dayLight.gameObject.SetActive(currentWeather == WeatherType.Sunny);
            rainLight.gameObject.SetActive(currentWeather == WeatherType.Rain);
            stormLight.gameObject.SetActive(currentWeather == WeatherType.Storm);

        }
    }

    public enum DayPhase
    {
        Dawn,
        Day,
        Dusk,
        Night
    }

    public enum WeatherType
    {
        Sunny,
        Rain,
        Storm,
        Overcast
    }
}