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
    }
}
