using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace HackedDesign
{
    public class WeaponsController : MonoBehaviour
    {
        [SerializeField] private Dictionary<WeaponType, bool> hasWeapon = new Dictionary<WeaponType, bool>();
        [SerializeField] private WeaponType currentWeapon = WeaponType.Punch;
        [SerializeField] private Animator animator;
        //[SerializeField] private Dictionary<WeaponType, List<GameObject>> weaponObjects;
        [SerializeField] private List<WeaponObjects> weaponObjects;
        [SerializeField] private GameObject spearPrefab;
        [SerializeField] private GameObject molotovPrefab;
        [SerializeField] private bool enemy = false;

        public Dictionary<WeaponType, bool> HasWeapon { get => hasWeapon; set => hasWeapon = value; }
        public WeaponType CurrentWeapon { get => currentWeapon; set => currentWeapon = value; }

        private bool lastPunch = false;
        private float lastAttack = float.MinValue;

        void Awake()
        {
            HasWeapon.Add(WeaponType.Punch, true);
            HasWeapon.Add(WeaponType.Knife, true);
            HasWeapon.Add(WeaponType.Spear, true);
            HasWeapon.Add(WeaponType.Rifle, true);
            HasWeapon.Add(WeaponType.Molotov, true);
        }

        void Start()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            UpdateWeaponObjects();
        }

        public void SwitchWeapon(int weapon)
        {
            SwitchWeapon((WeaponType)weapon);
        }

        public void SwitchWeapon(WeaponType weapon)
        {
            Debug.Log("Switching weapon to " + weapon.ToString());
            this.currentWeapon = weapon;
            UpdateWeaponObjects();
        }

        private void UpdateWeaponObjects()
        {
            foreach (var weapon in weaponObjects)
            {
                foreach (var obj in weapon.gameObjects)
                {
                    obj.SetActive(weapon.type == currentWeapon);

                }

                if (weapon.rig != null)
                {
                    weapon.rig.weight = weapon.type == currentWeapon ? 1 : 0;
                }

            }
        }

        public void Attack(Vector3 target)
        {
            TurnTowards(target);
            switch (currentWeapon)
            {
                case WeaponType.Punch:
                    AttackPunch();
                    break;
                case WeaponType.Knife:
                    AttackKnife();
                    break;
                case WeaponType.Spear:
                    AttackSpear(target);
                    break;
                case WeaponType.Rifle:
                    AttackRifle(target);
                    break;
                case WeaponType.Molotov:
                    AttackMolotov(target);
                    break;
                case WeaponType.Bite:
                    AttackBite(target);
                    break;
                case WeaponType.Claw:
                    AttackClaw(target);
                    break;
                default:
                    break;
            }
        }

        private void TurnTowards(Vector3 target)
        {
            var rotation = Quaternion.LookRotation(target - transform.position, Vector3.up);
            var targetAngle = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            transform.rotation = targetAngle; // Quaternion.Lerp(transform.rotation, targetAngle, rotateSpeed * Time.deltaTime);            
        }

        private void SelectNextHighestAvailableWeapon(WeaponType currentWeapon)
        {

        }

        public void AttackBite(Vector3 target)
        {
            if(lastAttack + Game.Instance.Settings.spearSpeed > Time.time)
            {
                return;
            }

            lastAttack = Time.time;

            Debug.Log("Attack Player");
        }

        public void AttackClaw(Vector3 target)
        {
            if(lastAttack + Game.Instance.Settings.spearSpeed > Time.time)
            {
                return;
            }

            lastAttack = Time.time;

            Debug.Log("Attack Player");
        }

        private void AttackSpear(Vector3 target)
        {
            if(lastAttack + Game.Instance.Settings.spearSpeed > Time.time)
            {
                return;
            }

            lastAttack = Time.time;

            HasWeapon[WeaponType.Spear] = false;
            var weaponObject = weaponObjects.First(e => e.type == WeaponType.Spear);

            var rotation = Quaternion.LookRotation(target - weaponObject.spawnPoint.position, Vector3.up);
            var go = Instantiate(spearPrefab, weaponObject.spawnPoint.position, rotation);
            var rb = go.GetComponent<Rigidbody>();
            rb.AddForce(target - weaponObject.spawnPoint.position, ForceMode.Impulse);
            animator.SetTrigger("Spear Throw");
            SelectNextHighestAvailableWeapon(WeaponType.Spear);

        }

        private void AttackRifle(Vector3 target)
        {
            if(lastAttack + Game.Instance.Settings.spearSpeed > Time.time)
            {
                return;
            }

            lastAttack = Time.time;
        }

        private void AttackMolotov(Vector3 target)
        {
            if(lastAttack + Game.Instance.Settings.molotovSpeed > Time.time)
            {
                return;
            }

            lastAttack = Time.time;

            HasWeapon[WeaponType.Molotov] = false;
            var weaponObject = weaponObjects.First(e => e.type == WeaponType.Molotov);

            var go = Instantiate(molotovPrefab, weaponObject.spawnPoint.position, Quaternion.identity);
            var rb = go.GetComponent<Rigidbody>();
            rb.AddForce(target - weaponObject.spawnPoint.position, ForceMode.Impulse);
            animator.SetTrigger("Spear Throw");
            SelectNextHighestAvailableWeapon(WeaponType.Molotov);
        }

        private void AttackKnife()
        {
            if(lastAttack + Game.Instance.Settings.knifeSpeed > Time.time)
            {
                return;
            }

            lastAttack = Time.time;


            lastPunch = !lastPunch;

            if (Random.value < 0.2f) // small chance to repeat
            {
                lastPunch = !lastPunch;
            }


            if (lastPunch)
            {
                animator.SetTrigger("Knife Swing 1");
            }
            else
            {
                animator.SetTrigger("Knife Swing 2");
            }
        }

        private void AttackPunch()
        {
            if(lastAttack + Game.Instance.Settings.punchSpeed > Time.time)
            {
                return;
            }

            lastAttack = Time.time;

            lastPunch = !lastPunch;

            Debug.Log("Punch");

            if (Random.value < 0.2f) // small chance to repeat
            {
                lastPunch = !lastPunch;
            }


            if (lastPunch)
            {
                animator.SetTrigger("Punch Left");
            }
            else
            {
                animator.SetTrigger("Punch Right");
            }
        }


    }

    [System.Serializable]
    public class WeaponObjects
    {
        public WeaponType type;
        public List<GameObject> gameObjects;
        public Rig rig;
        public Transform spawnPoint;
    }
}