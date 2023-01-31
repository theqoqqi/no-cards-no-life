using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Components.Scenes;
using Core.Cards.Stats;
using Core.Events;
using Core.GameStates;
using Core.Saves;
using UnityEngine;

namespace Components {
    public class Game : MonoBehaviour {

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
            if (Application.isEditor && Input.GetKeyDown(KeyCode.Delete)) {
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