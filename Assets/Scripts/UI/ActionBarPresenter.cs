using UnityEngine;
using TMPro;

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
        [SerializeField] private TMP_Text spearsCounterText;
        [SerializeField] private TMP_Text bulletsCounterText;
        [SerializeField] private TMP_Text molotovCounterText;

        new void Awake()
        {
            base.Awake();
            if (weapon1Button == null) Debug.LogError("weapon1Button not set", this);
            if (weapon2Button == null) Debug.LogError("weapon2Button not set", this);
            if (weapon3Button == null) Debug.LogError("weapon3Button not set", this);
            if (weapon4Button == null) Debug.LogError("weapon4Button not set", this);
            if (weapon5Button == null) Debug.LogError("weapon5Button not set", this);
            if (campButton == null) Debug.LogError("campButton not set", this);
            if (spearsCounterText == null) Debug.LogError("spearsCounterText not set", this);
            if (bulletsCounterText == null) Debug.LogError("bulletsCounterText not set", this);
            if (molotovCounterText == null) Debug.LogError("molotovCounterText not set", this);
        }

        public override void Repaint()
        {
            campButton.interactable = GameData.Instance.inShade;
            weapon1Button.interactable = Game.Instance.Player.Weapons.HasWeapon[WeaponType.Punch];
            weapon2Button.interactable = Game.Instance.Player.Weapons.HasWeapon[WeaponType.Knife];
            weapon3Button.interactable = Game.Instance.Player.Weapons.HasWeapon[WeaponType.Spear] && GameData.Instance.spears > 0;
            weapon4Button.interactable = Game.Instance.Player.Weapons.HasWeapon[WeaponType.Rifle] && GameData.Instance.bullets > 0;
            weapon5Button.interactable = Game.Instance.Player.Weapons.HasWeapon[WeaponType.Molotov] && GameData.Instance.molotovs > 0;

            switch (Game.Instance.Player.Weapons.CurrentWeapon)
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

        public void Weapon1Click() => Game.Instance.Player.SelectWeapon(WeaponType.Punch);
        public void Weapon2Click() => Game.Instance.Player.SelectWeapon(WeaponType.Knife);
        public void Weapon3Click() => Game.Instance.Player.SelectWeapon(WeaponType.Spear);
        public void Weapon4Click() => Game.Instance.Player.SelectWeapon(WeaponType.Rifle);
        public void Weapon5Click() => Game.Instance.Player.SelectWeapon(WeaponType.Molotov);
        public void CampClick() => Game.Instance.Player.ToggleCamp();
    }
}