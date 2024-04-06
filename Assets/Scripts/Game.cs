using Unity.VisualScripting;
using UnityEngine;

namespace HackedDesign
{
    public class Game : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Camera menuCamera;
        [SerializeField] private PlayerController player;
        [SerializeField] private MapManager level;
        [SerializeField] private DayManager dayManager;
        [SerializeField] private EnemyPool enemyPool;

        [Header("Data")]
        [SerializeField] private GameData gameData;
        [SerializeField] private Settings settings;

        [Header("UI")]
        [SerializeField] private UI.MainMenuPresenter mainMenuPanel = null;
        [SerializeField] private UI.IntroPresenter introPanel = null;
        [SerializeField] private UI.DayPresenter dayPanel = null;
        [SerializeField] private UI.CharPresenter charPanel = null;
        [SerializeField] private UI.ActionBarPresenter actionBarPanel = null;


        private IState state = new EmptyState();

        public static Game Instance { get; private set; }

        public IState State
        {
            get
            {
                return this.state;
            }
            private set
            {
                if(this.state != null)
                {
                    this.state.End();
                }
                this.state = value;
                if(this.state != null)
                {
                    this.state.Begin();
                }
            }
        }    

        public PlayerController Player { get { return player; } private set { player = value; } }    
        public MapManager Level { get { return level; } private set { level = value; } }

        public GameData GameData { get => gameData; set => gameData = value; }
        public Settings Settings { get => settings; set => settings = value; }
        public Camera MainCamera { get => mainCamera; set => mainCamera = value; }

        Game()
        {
            Instance = this;
        }        

        void Awake() => CheckBindings();
        void Start() => Initialization();
        void Update() => state.Update();
        void FixedUpdate() => state.FixedUpdate();

        public void SetMainMenu() => State = new MainMenuState(mainCamera, menuCamera, mainMenuPanel);
        public void SetIntro() => State = new IntroState(introPanel);
        public void SetLoading() => State = new LoadingState(Player, Level);
        public void SetPlaying() => State = new PlayingState(Player, dayManager, enemyPool, dayPanel, charPanel, actionBarPanel);
        private void SetDead() => State = new DeadState(Player, level);


        public void Quit()
        {
            Application.Quit();
        } 

        public void SelectCharacter(bool character)
        {
            Debug.Log("Select character " + character);
            this.GameData.chosenCharacter = character;
            this.Player.SwitchCharacter(character);
        }

        public void Die(DeathReason reason)
        {
            this.GameData.deathReason = reason;
            SetDead();
        }

        public void Reset()
        {
            GameData.Reset(Settings);
        }

        private void CheckBindings()
        {
        }

        private void Initialization()
        {
            this.level.gameObject.SetActive(false);
            Reset();
            SetMainMenu();
            //SetLoading();
        }
    }
}