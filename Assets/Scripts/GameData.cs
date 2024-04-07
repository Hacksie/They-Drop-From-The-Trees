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

        public WeatherType currentWeather = WeatherType.Overcast;
        public bool chosenCharacter = false;
        public float health = 100;
        public float sunburn = 0;
        public float hydration = 100;
         public DeathReason deathReason;
        public int shadeCount = 0;
        public bool inShade = false;
        public bool isCamping = false;
        public int bullets = 0;
        public int molotovs = 0;
        public int spears = 0;

        public Dictionary<string, int> killCounter = new Dictionary<string, int>();

        public void Reset(Settings settings)
        {
            currentWeather = WeatherType.Overcast;
            health = settings.startingHealth;
            sunburn = settings.startingSunburn;
            hydration = settings.startingHydration;
            deathReason = DeathReason.NotDead;
            spears = settings.startingSpears;
            bullets = settings.startingBullets;
            molotovs = settings.startingMolotovs;
            inShade = false;
            isCamping = false;
            killCounter = new Dictionary<string, int>() { {"Black Snake", 0} , {"Brown Snake", 0}, {"Emu", 0}, {"Kangaroo", 0}, {"Croc", 0}, {"DropBear", 0}, {"Yowie", 0}};
        }
    }

    public enum DeathReason 
    {
        NotDead,
        BurntToCrisp,
        DiedOfThirst,
        Bitten,
        Clawed,
        BurntAlive,
        Killed
    }
}