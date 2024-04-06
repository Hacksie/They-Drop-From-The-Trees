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
            HasWeapon.Add(WeaponType.Knife, false);
            HasWeapon.Add(WeaponType.Spear, false);
            HasWeapon.Add(WeaponType.Rifle, false);
            HasWeapon.Add(WeaponType.Molotov, false);
            HasWeapon.Add(WeaponType.Claw, true);
            HasWeapon.Add(WeaponType.Bite, true);
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
                    AttackPunch(target);
                    break;
                case WeaponType.Knife:
                    AttackKnife(target);
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

        public WeaponSettings GetWeaponSettings(WeaponType weapon) => Game.Instance.Settings.weaponSettings.FirstOrDefault(w => w.type == weapon);


        public void AttackBite(Vector3 target)
        {
            AttackMelee(WeaponType.Bite, target);
        }

        public void AttackClaw(Vector3 target)
        {
            AttackMelee(WeaponType.Claw, target);
        }

        private void AttackPunch(Vector3 target)
        {
            AttackMelee(WeaponType.Punch, target);

            // Animate ->
            lastPunch = !lastPunch;

            if (Random.value < 0.2f) // small chance to repeat
            {
                lastPunch = !lastPunch;
            }

            if (animator != null && animator.hasBoundPlayables)
            {
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

        private void AttackKnife(Vector3 target)
        {
            AttackMelee(WeaponType.Knife, target);

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

        private void AttackMelee(WeaponType type, Vector3 target)
        {
            var setting = GetWeaponSettings(type);
            if (setting == null)
            {
                Debug.LogError("No weapon settings for: " + type, this);
            }

            if (lastAttack + setting.speed > Time.time)
            {
                return;
            }

            lastAttack = Time.time;

            var miss = Random.value < Game.Instance.Settings.missChance;


            if (enemy)
            {
                Debug.Log("Attack player");
                var damage = Random.Range(setting.minDamage, setting.maxDamage);
                AttackPlayer(this.name, type, target, setting.distance, setting.missChance, damage);
            }
            else
            {
                Debug.Log("Attack enemy");
                var damage = Random.Range(setting.minDamage, setting.maxDamage);
                AttackEnemy("Player", type, target, setting.distance, setting.missChance, damage);
            }


        }

        private void AttackSpear(Vector3 target)
        {
            if (lastAttack + Game.Instance.Settings.spearSpeed > Time.time)
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
            if (lastAttack + Game.Instance.Settings.spearSpeed > Time.time)
            {
                return;
            }

            lastAttack = Time.time;

            if (enemy)
            {
                //FIXME: var damage = Random.Range(setting.minDamage, setting.maxDamage);
                AttackPlayer(this.name, WeaponType.Rifle, target, Game.Instance.Settings.punchDistance, 0, 20);
            }


            //var results = TargetsInLine(target, Game.Instance.Settings.rifleDistance);
        }

        private void AttackMolotov(Vector3 target)
        {
            if (lastAttack + Game.Instance.Settings.molotovSpeed > Time.time)
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

        private void AttackPlayer(string name, WeaponType type, Vector3 target, float range, float missChance, int amount)
        {
            var results = TargetsInLine(target, range);
            foreach (var hit in results)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    if (Random.value > missChance)
                    {
                        Debug.Log("Hit player");
                        Damageable d = Game.Instance.Player.GetComponent<Damageable>();
                        d.Damage(name, type, amount);
                        DamageNumbersPool.Instance.Spawn(target + Vector3.up, (target - transform.position).normalized, amount.ToString());
                    }
                    else
                    {
                        DamageNumbersPool.Instance.Spawn(target + Vector3.up, (target - transform.position).normalized, "miss");
                    }

                    return; // Just in case it hit the player multiple times
                }
            }
        }

        private void AttackEnemy(string name, WeaponType type, Vector3 target, float range, float missChance, int amount)
        {
            var results = TargetsInLine(target, range);
            if(results.Length == 0)
            {
                return;
            }
            var first = results.FirstOrDefault(e => e.collider.CompareTag("Enemy"));

            if(first.collider != null)
            {
                if (Random.value > missChance)
                    {
                        Debug.Log("Hit enemy");
                        Damageable d = Game.Instance.Player.GetComponent<Damageable>();
                        d.Damage(name, type, amount);
                        DamageNumbersPool.Instance.Spawn(target + Vector3.up, (target - transform.position).normalized, amount.ToString());
                    }
                    else
                    {
                        DamageNumbersPool.Instance.Spawn(target + Vector3.up, (target - transform.position).normalized, "miss");
                    }
            }
            /*
            foreach (var hit in results)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    if (Random.value > missChance)
                    {
                        Debug.Log("Hit player");
                        Damageable d = Game.Instance.Player.GetComponent<Damageable>();
                        d.Damage(name, type, amount);
                        DamageNumbersPool.Instance.Spawn(target + Vector3.up, (target - transform.position).normalized, amount.ToString());
                    }
                    else
                    {
                        DamageNumbersPool.Instance.Spawn(target + Vector3.up, (target - transform.position).normalized, "miss");
                    }

                    return; // Just in case it hit the player multiple times
                }
            }*/
        }

        private RaycastHit[] TargetsInLine(Vector3 target, float range)
        {
            var ray = new Ray(transform.position, target - transform.position);
            RaycastHit[] hitInfo = Physics.SphereCastAll(ray, 1, range);
            return hitInfo;
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