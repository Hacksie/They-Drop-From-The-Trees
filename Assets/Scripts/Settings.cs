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

        public float sunburnRate = 1f;
        public float hydrationLossPerSecondNormal = 0.04f;
        public float hydrationLossMultiplier = 2;
        public float secondsMultiplier = 48;
        public float sleepSecondsMultiplier = 4;
        public float startingTime = 800;
        public float dawn = 450;
        public float midday = 750;
        public float dusk = 1275;
        public float night = 1500;

        public float punchSpeed = 0.3f;
        public float knifeSpeed = 0.2f;
        public float spearSpeed = 0.5f;
        public float rifleSpeed = 1.0f;
        public float molotovSpeed = 1.0f;

        public float treeEnemyChance = 0.25f;

        public float rainChance = 0.1f;
        public float stormChance = 0.05f;

        public Vector3 playerSpawn = new Vector3(100,0,100);

        public bool invulnerable = false;
        
    }
}