using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign
{
    [System.Serializable]
    public class GameData
    {
        public static GameData Instance { get; private set; }

        public GameData()
        {
            Instance = this;
        }           

        public WeatherType currentWeather = WeatherType.Sunny;
        public bool chosenCharacter = false;
        public float health = 100;
        public float sunburn = 0;
        public float hydration = 100;
        public int currentDay = 1;
        public float currentTime = 0;
        public DeathReason deathReason;
        public int shadeCount = 0;
        public bool inShade = false;
        public bool isCamping = false;
        public int bullets = 0;
        public int molotovs = 0;
        public int spears = 0;

        public void Reset(Settings settings)
        {
            currentWeather = WeatherType.Sunny;
            health = settings.startingHealth;
            sunburn = settings.startingSunburn;
            hydration = settings.startingHydration;
            currentDay = 1;
            currentTime = settings.startingTime;
            deathReason = DeathReason.NotDead;
            bullets = 0;
            molotovs = 0;
            spears = 0;
        }
    }

    public enum DeathReason 
    {
        NotDead,
        BurntToCrisp,
        DiedOfThirst,
        BittenBySnake,
        BurntAlive,
        Killed
    }
}