using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.UI
{
    public class ActionBarPresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Button weapon1Button;
        [SerializeField] private UnityEngine.UI.Button weapon2Button;
        [SerializeField] private UnityEngine.UI.Button weapon3Button;
        [SerializeField] private UnityEngine.UI.Button weapon4Button;
        [SerializeField] private UnityEngine.UI.Button weapon5Button;
        [SerializeField] private UnityEngine.UI.Button campButton;

        public override void Repaint()
        {
            campButton.interactable = GameData.Instance.inShade;
            weapon1Button.interactable = Game.Instance.Player.Weapons.HasWeapon[WeaponType.Punch];
            weapon2Button.interactable = Game.Instance.Player.Weapons.HasWeapon[WeaponType.Knife];
            weapon3Button.interactable = Game.Instance.Player.Weapons.HasWeapon[WeaponType.Spear];
            weapon4Button.interactable = Game.Instance.Player.Weapons.HasWeapon[WeaponType.Rifle];
            weapon5Button.interactable = Game.Instance.Player.Weapons.HasWeapon[WeaponType.Molotov];
            
            switch(Game.Instance.Player.Weapons.CurrentWeapon)
            {
                case WeaponType.Punch:
                default:
                    weapon1Button.Select();
                    break;
                case WeaponType.Knife:
                    weapon2Button.Select();
                    break;
                case WeaponType.Spear:
                    weapon3Button.Select();
                    break;
                case WeaponType.Rifle:
                    weapon4Button.Select();
                    break;
                case WeaponType.Molotov:
                    weapon5Button.Select();
                    break;
            }
            
        }

        public void Weapon1Click()
        {
            Game.Instance.Player.SelectWeapon(WeaponType.Punch);
        }

        public void Weapon2Click()
        {
            Game.Instance.Player.SelectWeapon(WeaponType.Knife);
        }

        public void Weapon3Click()
        {
            Game.Instance.Player.SelectWeapon(WeaponType.Spear);
        }                

        public void Weapon4Click()
        {
            Game.Instance.Player.SelectWeapon(WeaponType.Rifle);
        }

        public void Weapon5Click()
        {
            Game.Instance.Player.SelectWeapon(WeaponType.Molotov);
        }        

        public void CampClick()
        {
            Game.Instance.Player.ToggleCamp();
        }        
    }
}