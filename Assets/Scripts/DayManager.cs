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

        public static DayManager Instance { get; private set; }

        private DayPhase currentDayPhase = DayPhase.Day;

        DayManager()
        {
            Instance = this;
        }         

        public void UpdateBehaviour()
        {
            var newDayPhase = GetDayPhase(GameData.Instance.currentTime, Game.Instance.Settings);
            if(newDayPhase != this.currentDayPhase)
            {
                Debug.Log("Switch weather");
                // Calc weather
                var r = Random.value;
                if(r < Game.Instance.Settings.stormChance)
                {
                    GameData.Instance.currentWeather = WeatherType.Storm;
                }
                else if (r < Game.Instance.Settings.rainChance)
                {
                    GameData.Instance.currentWeather = WeatherType.Rain;
                }
                else
                {
                    GameData.Instance.currentWeather = WeatherType.Sunny;
                }
            }
            this.currentDayPhase = newDayPhase;

            UpdateTime();
            CheckNextDay();
            UpdateSunburn(this.currentDayPhase);
            UpdateHydration();
            UpdateLight();
        }

        private void UpdateTime()
        {
            GameData.Instance.currentTime += Time.deltaTime * Game.Instance.Settings.secondsMultiplier * (GameData.Instance.isCamping? Game.Instance.Settings.sleepSecondsMultiplier : 1);
        }

        private void CheckNextDay()
        {
            if (GameData.Instance.currentTime > secondsPerDay)
            {
                var leftOver = GameData.Instance.currentTime * Game.Instance.Settings.secondsMultiplier - secondsPerDay;
                GameData.Instance.currentTime = leftOver;
                GameData.Instance.currentDay++;
            }
        }        


        private void UpdateSunburn(DayPhase dayPhase)
        {
            if (currentDayPhase == DayPhase.Day && !GameData.Instance.inShade)
            {
                GameData.Instance.sunburn += Time.deltaTime * Game.Instance.Settings.sunburnRate;
                GameData.Instance.sunburn = Mathf.Clamp(GameData.Instance.sunburn, 0, Game.Instance.Settings.maxSunburn);
            }

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
            var phase = GetDayPhase(GameData.Instance.currentTime, Game.Instance.Settings);

            dawnLight.gameObject.SetActive(phase == DayPhase.Dawn && GameData.Instance.currentWeather == WeatherType.Sunny);
            dayLight.gameObject.SetActive(phase == DayPhase.Day  && GameData.Instance.currentWeather == WeatherType.Sunny);
            duskLight.gameObject.SetActive(phase == DayPhase.Dusk && GameData.Instance.currentWeather == WeatherType.Sunny);
            stormLight.gameObject.SetActive(GameData.Instance.currentWeather != WeatherType.Sunny && phase != DayPhase.Night);
            nightLight.gameObject.SetActive(phase == DayPhase.Night);
            
        }

        public static DayPhase GetDayPhase(float time, Settings settings)
        {
            if(time < settings.dawn)
            {
                return DayPhase.Night;
            }
            else if (time < settings.midday)
            {
                return DayPhase.Dawn;
            }
            else if (time < settings.dusk)
            {
                return DayPhase.Day;
            }
            else if (time < settings.night)
            {
                return DayPhase.Dusk;
            }
            else
            {
                return DayPhase.Night;
            }
        }
    }

    public enum DayPhase {
        Dawn,
        Day,
        Dusk,
        Night
    }

    public enum WeatherType 
    {
        Sunny,
        Rain,
        Storm
    }
}