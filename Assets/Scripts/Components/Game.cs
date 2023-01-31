using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Components.Scenes;
using Core.Cards;
using Core.Cards.Stats;
using Core.Events;
using Core.Events.Levels;
using Core.GameStates;
using Core.Saves;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Components {
    public class Game : MonoBehaviour {

        /*
         * Наверное, игровые данные должны просто храниться в компоненте (ну, в каком-нибудь GameManager),
         * а система загрузки/сохранений уже просто напрямую из этого компонента берет необходимые данные и сохраняет.
         *
         * К примеру, в GameManager может храниться Deck CurrentDeck, List<Card> UnlockedCards, int deaths и т.д.
         * То есть это могут быть любые данные, включая мои кастомные классы. Так будет достигнуто следующее:
         * 1. Все данные хранятся в централизованном хранилище (GameManager)
         * 2. Можно свободно создавать иерархию собственных классов (Deck, Card и т.д.)
         * 3. Система загрузки/сохранений может просто обращаться к GameManager, из которого будет брать данные
         * 4. Использование системы можно будет свести к вызовам методов NewGame/SaveGame/LoadGame/ResetGame и т.д.
         *
         * Вроде бы получается достаточно чистая и минимально связанная архитектура.
         * 
         * Возможно еще понадобится добавить классы типа SaveGameData, чтобы отделить систему З/С от компонентов.
         * Тогда получится так: GameManager -> SaveGameData -> SaveLoadManager.
         * Это позволит отделить чтение данных из игры от записи данных в файл и наоборот.
         */

        public static Game Instance { get; private set; }

        public static bool IsBootstrapDone => Instance;

        public static event Action BootstrapDone;

        private static bool loadingFromBootstrap;

        private bool isGoingToReset;

        public GameState GameState { get; private set; }

        [SerializeField] private SceneManager sceneManager;

        public SceneManager SceneManager => sceneManager;

        private async void Awake() {
            if (!SetupInstance()) {
                return;
            }

            SetupSettings();

            if (!loadingFromBootstrap) {
                await sceneManager.LoadMainMenu();
                LoadCurrentSave();
            }
        }

        private bool SetupInstance() {
            if (Instance) {
                Destroy(gameObject);
                return false;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;

            return true;
        }

        private static void SetupSettings() {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        private void OnEnable() {
            GameEvents.Instance.On<LevelDoneEvent>(OnLevelDone);
            GameEvents.Instance.On<LevelFailedEvent>(OnLevelFailed);
        }

        private void OnDisable() {
            GameEvents.Instance.Off<LevelDoneEvent>(OnLevelDone);
            GameEvents.Instance.Off<LevelFailedEvent>(OnLevelFailed);
        }

        private async void OnLevelDone(LevelDoneEvent e) {
            var newCard = CreateRandomCard();

            GameState.StarterDeck.AddCard(newCard);

            await sceneManager.LoadLocationMap();
        }

        private async void OnLevelFailed(LevelFailedEvent e) {
            var newCard = CreateRandomCard();

            GameState.StarterDeck.AddCard(newCard);
            GameState.StartNewRun();

            await sceneManager.LoadDeathScreen();
        }

        private void LateUpdate() {
            GameEvents.Instance.DispatchEnqueuedEvents();
        }

        private void OnApplicationQuit() {
            if (!loadingFromBootstrap && !isGoingToReset) {
                Save();
            }
        }

        private void LoadCurrentSave() {
            if (SaveLoadSystem.HasSave) {
                Load();
            }
            else {
                LoadDefaultSave();
            }
        }

        private void LoadDefaultSave() {
            var gameSave = new GameSaveData();

            gameSave.firstRunDeck = CreateTestDeck();

            Load(gameSave);
        }

        private static CardStats[] CreateTestDeck() {
            // Это потом можно будет вынести в сериализуемое поле этого класса (Game.firstRunDeck или Game.defaultSave)
            return new CardStats[] {
                    new MoveCardStats(1),
                    new MoveCardStats(2),
                    new AttackCardStats(1, 1),
            };
        }

        private void Save() {
            SaveLoadSystem.SaveGame(GameState);
        }

        private void Load() {
            GameState = SaveLoadSystem.LoadGame();
        }

        private void Load(GameSaveData gameSave) {
            if (gameSave == null) {
                LoadDefaultSave();
                return;
            }

            GameState = SaveLoadSystem.Load(gameSave);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Delete)) {
                ResetToFirstRun();
            }
        }

        private void ResetToFirstRun() {
            isGoingToReset = true;
            SaveLoadSystem.DeleteSave();
            RestartApp();
        }

        private static void RestartApp() {
            if (Application.isEditor) {
                Application.Quit();
                return;
            }
            
            var executableFileName = GetExecutableFileName();
            
            Process.Start(executableFileName);
            Application.Quit();
        }

        private static string GetExecutableFileName() {
            return Application.dataPath.Replace("_Data", ".exe");
        }

        private static Card CreateRandomCard() {
            if (Random.Range(0, 2) == 0) {
                var maxDistance = Random.Range(1, 4);

                return new MoveCard(maxDistance);
            }
            else {
                var maxDistance = Random.Range(1, 3);
                var damage = Random.Range(1, 3) + Random.Range(0, 2);

                return new AttackCard(maxDistance, damage);
            }
        }

        public static async Task<Game> Bootstrap(GameSaveData saveData) {
            loadingFromBootstrap = true;

            SceneManager.LoadSystemScene();

            while (!Instance) {
                await Task.Yield();
            }

            Instance.Load(saveData);

            BootstrapDone?.Invoke();

            return Instance;
        }
    }
}