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
            HasWeapon.Add(WeaponType.Spear, true);
            HasWeapon.Add(WeaponType.Rifle, true);
            HasWeapon.Add(WeaponType.Molotov, true);
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
            if (AttackMelee(WeaponType.Bite, target))
            {
                // FIXME: Animate
            }
        }

        public void AttackClaw(Vector3 target)
        {
            if (AttackMelee(WeaponType.Claw, target))
            {
                // FIXME: Animate
            }
        }

        private void AttackPunch(Vector3 target)
        {
            if (AttackMelee(WeaponType.Punch, target))
            {
                AnimatePunch();
            }
        }

        private void AnimatePunch()
        {
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
            if (AttackMelee(WeaponType.Knife, target))
            {
                AnimateKnife();
            }
        }

        private void AnimateKnife()
        {
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

        private bool AttackMelee(WeaponType type, Vector3 target)
        {
            var setting = GetWeaponSettings(type);
            if (setting == null)
            {
                Debug.LogError("No weapon settings for: " + type, this);
            }

            if (lastAttack + setting.speed > Time.time)
            {
                return false;
            }

            lastAttack = Time.time;

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

            return true;
        }

        private void AttackSpear(Vector3 target)
        {
            var setting = GetWeaponSettings(WeaponType.Spear);
            if (setting == null)
            {
                Debug.LogError("No weapon settings for: " + WeaponType.Spear, this);
            }

            if (lastAttack + setting.speed > Time.time)
            {
                return;
            }

            lastAttack = Time.time;
            target += Vector3.up; // Aim a little higher, to compensate for the fact that we're targeting the ground
            var weaponObject = weaponObjects.First(e => e.type == WeaponType.Spear);

            var rotation = Quaternion.LookRotation(target - weaponObject.spawnPoint.position, Vector3.up);
            EntityPool.Instance.SpawnSpear( weaponObject.spawnPoint.position, (target - weaponObject.spawnPoint.position) * 2, rotation);
            animator.SetTrigger("Spear Throw");
            SelectNextHighestAvailableWeapon(WeaponType.Spear);
        }

        private void AttackRifle(Vector3 target)
        {
            var setting = GetWeaponSettings(WeaponType.Rifle);
            if (setting == null)
            {
                Debug.LogError("No weapon settings for: " + WeaponType.Rifle, this);
            }

            if (lastAttack + setting.speed > Time.time)
            {
                return;
            }

            lastAttack = Time.time;

            if (enemy)
            {
                var damage = Random.Range(setting.minDamage, setting.maxDamage);
                AttackPlayer(this.name, WeaponType.Rifle, target, setting.distance, setting.missChance, damage);
            }
            else
            {
                Debug.Log("Attack enemy");
                GameData.Instance.bullets--;
                GameData.Instance.bullets = Mathf.Max(GameData.Instance.bullets, 0);
                var damage = Random.Range(setting.minDamage, setting.maxDamage);
                AttackEnemy("Player", WeaponType.Rifle, target, setting.distance, setting.missChance, damage);
            }
        }

        private void AttackMolotov(Vector3 target)
        {
            var setting = GetWeaponSettings(WeaponType.Molotov);
            if (setting == null)
            {
                Debug.LogError("No weapon settings for: " + WeaponType.Molotov, this);
            }

            if (lastAttack + setting.speed > Time.time)
            {
                return;
            }

            lastAttack = Time.time;

            GameData.Instance.molotovs--;
            GameData.Instance.molotovs = Mathf.Max(GameData.Instance.bullets, 0);

            var weaponObject = weaponObjects.First(e => e.type == WeaponType.Molotov);
            target += Vector3.up;

            var rotation = Quaternion.LookRotation(target - weaponObject.spawnPoint.position, Vector3.up);
            EntityPool.Instance.SpawnMolotov(weaponObject.spawnPoint.position, (target - weaponObject.spawnPoint.position) + Vector3.up * 4, rotation);

            //var go = Instantiate(molotovPrefab, weaponObject.spawnPoint.position, Quaternion.identity);
            //var rb = go.GetComponent<Rigidbody>();
            //rb.AddForce(target - weaponObject.spawnPoint.position, ForceMode.Impulse);
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
            if (results.Length == 0)
            {
                DamageNumbersPool.Instance.Spawn(target + Vector3.up, (target - transform.position).normalized, "miss");
                return;
            }
            var first = results.FirstOrDefault(e => e.collider.CompareTag("Enemy"));

            if (first.collider != null)
            {
                if (Random.value > missChance)
                {
                    Debug.Log("Hit enemy");
                    Damageable d = first.collider.GetComponent<Damageable>();
                    d.Damage(name, type, amount);
                    EffectsPool.Instance.SpawnBloodSplatter(target, Random.rotation);
                    DamageNumbersPool.Instance.Spawn(target + Vector3.up, (target - transform.position).normalized, amount.ToString());
                }
                else
                {
                    DamageNumbersPool.Instance.Spawn(target + Vector3.up, (target - transform.position).normalized, "miss");
                }
            }
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