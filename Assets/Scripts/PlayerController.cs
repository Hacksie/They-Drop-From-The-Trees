using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace HackedDesign
{
    public class PlayerController : MonoBehaviour
    {
        [Header("GameObjects")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Agent agent;
        [SerializeField] private WeaponsController weapons;
        [SerializeField] private CampController camp;
        [SerializeField] private WeatherController weather;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private List<GameObject> maleCharacter;
        [SerializeField] private List<GameObject> femaleCharacter;
        [SerializeField] private MapManager mapManager;
        [SerializeField] private CameraShake shake;

        [Header("Settings")]
        [SerializeField] private LayerMask aimMask;
        [SerializeField] private bool character = true;
        [SerializeField] private Vector3 lookAtOffset = new Vector3(0, 2, 0);

        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction mousePosAction;
        private InputAction primaryAction;
        private InputAction secondaryAction;
        private InputAction weapon1Action;
        private InputAction weapon2Action;
        private InputAction weapon3Action;
        private InputAction weapon4Action;
        private InputAction weapon5Action;
        private InputAction menuAction;


        private Vector2 moveInput;
        private Vector3 lookAtDirection;


        private RaycastHit[] raycastHits = new RaycastHit[1];

        public WeaponsController Weapons { get => weapons; set => weapons = value; }

        void Awake()
        {
            if (agent == null)
            {
                agent = GetComponent<Agent>();
            }

            if (playerInput == null)
            {
                playerInput = GetComponent<PlayerInput>();
            }

            moveAction = playerInput.actions["Move"];
            lookAction = playerInput.actions["Look"];
            mousePosAction = playerInput.actions["Mouse Position"];
            primaryAction = playerInput.actions["Primary Action"];
            secondaryAction = playerInput.actions["Secondary Action"];
            weapon1Action = playerInput.actions["Weapon 1"];
            weapon2Action = playerInput.actions["Weapon 2"];
            weapon3Action = playerInput.actions["Weapon 3"];
            weapon4Action = playerInput.actions["Weapon 4"];
            weapon5Action = playerInput.actions["Weapon 5"];
            menuAction = playerInput.actions["Menu"];
            menuAction.performed += MenuEvent;
            weapon1Action.performed += Weapon1ActionEvent;
            weapon2Action.performed += Weapon2ActionEvent;
            weapon3Action.performed += Weapon3ActionEvent;
            weapon4Action.performed += Weapon4ActionEvent;
            weapon5Action.performed += Weapon5ActionEvent;
        }

        void Start()
        {
            Reset();
            agent.Reset();
        }

        public void Reset()
        {
            agent.Reset();
            SetCharacter();
            weather.UpdateWeather();
            gameObject.SetActive(false);
        }

        private void MenuEvent(InputAction.CallbackContext context)
        {
            Game.Instance.State.Menu();
        }

        private void Weapon1ActionEvent(InputAction.CallbackContext context)
        {
            weapons.SwitchWeapon(0);
        }

        private void Weapon2ActionEvent(InputAction.CallbackContext context)
        {
            weapons.SwitchWeapon(1);
        }

        private void Weapon3ActionEvent(InputAction.CallbackContext context)
        {
            weapons.SwitchWeapon(2);
        }

        private void Weapon4ActionEvent(InputAction.CallbackContext context)
        {
            weapons.SwitchWeapon(3);
        }

        private void Weapon5ActionEvent(InputAction.CallbackContext context)
        {
            weapons.SwitchWeapon(4);
        }

        public void Pickup()
        {
            agent.Pickup();
        }

        public void Damage(string attacker, WeaponType type, int amount)
        {
            Debug.Log("player took :" + type.ToString() + " damage: " + amount + " from: " + attacker);
            GameData.Instance.health -= amount;
            shake.Shake(0.5f, 0.2f);
            agent.TakeHit();


            if (GameData.Instance.health < 0)
            {
                GameData.Instance.health = 0;
    
                agent.Die();
                if (Game.Instance.Settings.invulnerable)
                {
                    Debug.Log("Player would have died but is invulnerable");
                }
                else
                {
                    Debug.Log("Player is dead");
                    Game.Instance.Die(attacker, CalcDeathReason(type));
                    // Game.Instance.SetDead();
                }
            }
        }

        private DeathReason CalcDeathReason(WeaponType type)
        {
            switch (type)
            {
                case WeaponType.Punch:
                    return DeathReason.Punched;
                case WeaponType.Fire:
                    return DeathReason.BurntAlive;
                case WeaponType.Bite:
                    return DeathReason.Bitten;
                case WeaponType.Claw:
                    return DeathReason.Clawed;
                case WeaponType.Lightning:
                    return DeathReason.Lightning;
            }

            return DeathReason.Killed;
        }

        public void SwitchCharacter(bool character)
        {
            this.character = character;
            SetCharacter();
        }

        public void Spawn(Vector3 position)
        {
            agent.TeleportTo(position);
        }

        public void UpdateBehaviour()
        {
            this.moveInput = moveAction.ReadValue<Vector2>();
            var mousePosition = GetMousePosition();
            var worldPos = GetWorldMousePosition(mousePosition);

            if (primaryAction.IsPressed() )
            {
                agent.MoveTo(worldPos);
            }

            if (secondaryAction.IsPressed() && !EventSystem.current.IsPointerOverGameObject())
            {
                weapons.Attack(worldPos);
            }

            /*
            if(playerInput.currentControlScheme == "Gamepad")
            {
                var look = lookAction.ReadValue<Vector2>();
                var lookDirection = Quaternion.AngleAxis(45, Vector3.up) * new Vector3(look.x, 0, look.y);
                //Vector3 lookDirection = Vector3.right * look.x + Vector3.forward * look.y;
                
                agent.Move(this.moveInput, lookDirection);
                if(lookDirection.magnitude > 0)
                {
                    this.lookAtDirection = lookDirection;
                }

                agent.LookAt = transform.position + (lookAtDirection * 20) + lookAtOffset;
                
            }
            else
            {
                agent.Move(this.moveInput, new Vector3(worldPos.x, 0, worldPos.z) - new Vector3(transform.position.x, 0, transform.position.z));
                agent.LookAt = worldPos + lookAtOffset;
            }*/

            agent.LookAt = worldPos + lookAtOffset;
            
            agent.UpdateBehaviour();
            weather.UpdateWeather();
            camp.UpdateBehaviour();
            


        }

        public void FixedUpdateBehaviour()
        {
            //this.moveInput = moveAction.ReadValue<Vector2>();
            //Debug.Log(this.moveInput);
            //agent.Move(moveInput);
        }

        public void SelectWeapon(int weapon)
        {
            SelectWeapon((WeaponType)weapon);
        }

        public void SelectWeapon(WeaponType weapon)
        {
            weapons.SwitchWeapon(weapon);
        }


        private void SetCharacter()
        {
            maleCharacter.ForEach(e => e.SetActive(!GameData.Instance.isCamping && !character));
            femaleCharacter.ForEach(e => e.SetActive(!GameData.Instance.isCamping && character));
        }

        private Vector2 GetMousePosition()
        {
            if (mousePosAction == null)
            {
                Debug.LogError("mousePosAction not set", this);
                return Vector2.zero;
            }
            return mousePosAction.ReadValue<Vector2>();
        }

        private Vector3 GetWorldMousePosition(Vector2 mousePosition)
        {
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.RaycastNonAlloc(ray, raycastHits, 100, aimMask) > 0)
            {
                return raycastHits[0].point;
            }

            return Vector3.zero;
        }
    }
}