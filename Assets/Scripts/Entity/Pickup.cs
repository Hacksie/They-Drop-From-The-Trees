using System.Collections;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] PickupType type;

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                Activate();
            }
        }

        private void Activate()
        {
            switch(type)
            {
                case PickupType.Knife:
                    KnifePickup();
                    //GameData.Instance.h
                break;
                case PickupType.Spear:
                    SpearPickup();
                    break;
                case PickupType.Bullet:
                    BulletPickup();
                    break;
                case PickupType.Molotov:
                    MolotovPickup();
                    break;
                case PickupType.Vegemite:
                    VegemitePickup();
                break;
            }

            gameObject.SetActive(false);
        }

        private void KnifePickup()
        {
            GameData.Instance.hasWeapon[WeaponType.Knife] = true;
            Game.Instance.Player.SelectWeapon(WeaponType.Knife);
        }

        private void SpearPickup()
        {
            GameData.Instance.hasWeapon[WeaponType.Spear] = true;
            GameData.Instance.spears+=2;
            Game.Instance.Player.SelectWeapon(WeaponType.Spear);
            //Game.Instance.Player.SelectNextHighestAvailableWeapon();
        }

        private void BulletPickup()
        {
            GameData.Instance.hasWeapon[WeaponType.Rifle] = true;
            GameData.Instance.bullets += 5;
            Game.Instance.Player.SelectWeapon(WeaponType.Rifle);
        }

        private void MolotovPickup()
        {
            GameData.Instance.hasWeapon[WeaponType.Molotov] = true;
            GameData.Instance.molotovs++;
            Game.Instance.Player.SelectWeapon(WeaponType.Molotov);
        }        

        private void VegemitePickup()
        {
            GameData.Instance.health = Game.Instance.Settings.maxHealth;
            GameData.Instance.sunburn = 0;
        }

    }


    public enum PickupType
    {
        Knife,
        Spear,
        Bullet,
        Molotov,
        Vegemite
    }
}