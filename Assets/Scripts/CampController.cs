using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace HackedDesign
{
    public class CampController : MonoBehaviour
    {
        [SerializeField] private GameObject campModel;
        [SerializeField] private GameObject playerModelParent;

        public void UpdateCamp()
        {
            campModel.SetActive(GameData.Instance.isCamping);
            playerModelParent.SetActive(!GameData.Instance.isCamping);
        }

        public void UpdateBehaviour()
        {
            //if(GameData.Instance.isCamping)
            if(GameData.Instance.inShade)
            {
                GameData.Instance.health += Game.Instance.Settings.healthRecoverRate * Time.deltaTime;
                GameData.Instance.health = Mathf.Clamp(GameData.Instance.health, 0, Game.Instance.Settings.maxHealth);
                if(GameData.Instance.health == 0)
                {
                    Game.Instance.SetDead(); 
                }
                GameData.Instance.sunburn -= Game.Instance.Settings.healthRecoverRate * Time.deltaTime;
                GameData.Instance.sunburn = Mathf.Clamp(GameData.Instance.sunburn, 0, Game.Instance.Settings.maxSunburn);
            }
        }
    }
}
