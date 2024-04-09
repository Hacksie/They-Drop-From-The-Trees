using UnityEngine;

namespace HackedDesign
{
    public class CampController : MonoBehaviour
    {
        public void UpdateBehaviour()
        {
            //if(GameData.Instance.isCamping)
            if(GameData.Instance.inShade)
            {
                GameData.Instance.health += Game.Instance.Settings.healthRecoverRate * Time.deltaTime;
                GameData.Instance.health = Mathf.Clamp(GameData.Instance.health, 0, Game.Instance.Settings.maxHealth);
                if(GameData.Instance.health == 0)
                {
                    Game.Instance.Die("The Sun", DeathReason.BurntToCrisp);
                }
                GameData.Instance.sunburn -= Game.Instance.Settings.healthRecoverRate * Time.deltaTime;
                GameData.Instance.sunburn = Mathf.Clamp(GameData.Instance.sunburn, 0, Game.Instance.Settings.maxSunburn);
              
            }
        }
    }
}
