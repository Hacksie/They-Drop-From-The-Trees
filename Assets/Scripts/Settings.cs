using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Settings", menuName = "State/Settings")]
    public class Settings : ScriptableObject
    {
        public int maxHealth = 100;
        public int maxSunburn = 100;
        public int maxHydration = 100;
        public int startingHealth = 100;
        public int startingSunburn = 0;
        public int startingHydration = 100;
        public int startingBullets = 0;
        public int startingSpears = 0;
        public int startingMolotovs = 0;

        public float healthRecoverRate = 1f;

        public float weatherChangeRate = 30f;
        
        public float hydrationLossPerSecondNormal = 0.04f;
        public float hydrationLossMultiplier = 2;
        public float sleepSecondsMultiplier = 4;

        public List<WeaponSettings> weaponSettings;
        public List<WeatherSettings> weatherSettings;

        public float treeEnemyChance = 0.5f;
        public float maxEnemiesPerTree = 3;

        public Vector3 playerSpawn = new Vector3(128,0,128);

        public bool invulnerable = false;
        public bool infiniteAmmo = false;
        public float spearTimeout = 5f;
        public float molotovTimeout = 2f;
        public float molotovRadius = 3f;

        public float fireTimeout = 2f;
        public float fireSpreadChance = 0.5f;
        public float smokeTimeout = 2f;
        public int maxFireSpawns = 20;

        public float treeRespawnMinTimeout = 10f;
        public float treeRespawnMaxTimeout = 30f;

        //public float waterCrocChance = 0.25f;
        
    }

    [System.Serializable]
    public class WeaponSettings
    {
        public WeaponType type;
        public float speed;
        public float distance;
        public float missChance;
        public int minDamage;
        public int maxDamage;
    }

    [System.Serializable]
    public class WeatherSettings
    {
        public WeatherType type;
        public float sunburnRate;
        public float fireSpreadChance;
    }
}