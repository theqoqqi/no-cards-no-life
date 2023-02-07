using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Scenes.Screens;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Components.Scenes {
    public class SceneManager : MonoBehaviour {

        public static string CurrentSceneName => UnitySceneManager.GetActiveScene().name;
        
        private const string ScreenScenesDirectory = "Scenes/Screens/";
        
        private const string LevelScenesDirectory = "Scenes/Levels/";
        
        private const string LocationScenesDirectory = "Scenes/Locations/";

        private const string SystemSceneName = "Scenes/SystemScene";

        private const string MainMenuScreenSceneName = "Scenes/Screens/MainMenuScreen";

        private const string LocationMapScreenSceneName = "Scenes/Screens/LocationMapScreen";

        private const string LevelScreenSceneName = "Scenes/Screens/LevelScreen";

        private const string DeathScreenSceneName = "Scenes/Screens/DeathScreen";

        private static IList<string> screenSceneNames;

        private static IList<string> levelSceneNames;

        private static IList<string> locationSceneNames;

        [SerializeField] private ScreenFader screenFader;

        [SerializeField] private Camera fallbackCamera;

        [SerializeField] private float fadeDuration = 1;

        private ScreenScene currentScreen;

        private string currentScreenSceneName;

        private string currentContentSceneName;

        private void Awake() {
            fallbackCamera.enabled = false;
        }

        public async Task LoadMainMenu() {
            await SwitchScreen(MainMenuScreenSceneName);
        }

        public async Task LoadLocationMap() {
            await SwitchScreen(LocationMapScreenSceneName);
        }

        public async Task LoadLevel(string levelSceneName) {
            await SwitchScreen(LevelScreenSceneName, levelSceneName);
        }

        public async Task LoadDeathScreen() {
            await SwitchScreen(DeathScreenSceneName);
        }

        private async Task SwitchScreen(string sceneName) {
            await SwitchScreen(sceneName, null);
        }

        private async Task SwitchScreen(string sceneName, string contentSceneName) {
            await FadeOut();
            await UnloadCurrentContentScene();
            await UnloadCurrentScreenScene();
            await LoadScreenScene(sceneName);
            await LoadContentScene(contentSceneName);
            await FadeIn();
        }

        public async Task AutoLoadScene(string sceneName) {
            var isScreenScene = screenSceneNames.Contains(ScreenScenesDirectory + sceneName);

            if (isScreenScene) {
                await LoadScreenScene(ScreenScenesDirectory + sceneName);
                return;
            }

            var isLevelScene = levelSceneNames.Contains(LevelScenesDirectory + sceneName);
            
            if (isLevelScene) {
                await LoadContentScene(LevelScenesDirectory + sceneName);
                return;
            }

            var isLocationScene = locationSceneNames.Contains(LocationScenesDirectory + sceneName);
            
            if (isLocationScene) {
                await LoadContentScene(LocationScenesDirectory + sceneName);
                return;
            }

            throw new Exception("Currently, autoload only supports screens and levels");
        }

        private async Task LoadScreenScene(string sceneName) {
            await LoadScene(sceneName);

            currentScreen = FindObjectOfType<ScreenScene>();
            currentScreenSceneName = sceneName;
        }

        private async Task LoadContentScene(string sceneName) {
            if (sceneName == null) {
                return;
            }

            await LoadScene(sceneName);

            currentContentSceneName = sceneName;
        }

        private async Task UnloadCurrentContentScene() {
            if (currentContentSceneName == null) {
                return;
            }

            await UnloadScene(currentContentSceneName);

            currentContentSceneName = null;
        }

        private async Task UnloadCurrentScreenScene() {
            if (!currentScreen) {
                return;
            }

            currentScreen.Unload();

            await UnloadScene(currentScreenSceneName);

            currentScreenSceneName = null;
        }

        private async Task FadeOut() {
            Cursor.lockState = CursorLockMode.Locked;
            
            await screenFader.FadeOut(fadeDuration);
            
            fallbackCamera.enabled = true;
        }

        private async Task FadeIn() {
            fallbackCamera.enabled = false;
            
            await screenFader.FadeIn(fadeDuration);
            
            Cursor.lockState = CursorLockMode.None;
        }

        public static void InitAutoLoad() {
            var scenePaths = GetAllScenePaths();
            
            IList<string> PathsIn(string directory) {
                return scenePaths.Where(path => path.StartsWith(directory)).ToList();
            }

            screenSceneNames = PathsIn(ScreenScenesDirectory);
            levelSceneNames = PathsIn(LevelScenesDirectory);
            locationSceneNames = PathsIn(LocationScenesDirectory);
        }

        public static void LoadSystemScene() {
            UnitySceneManager.LoadScene(SystemSceneName);
        }

        private static async Task LoadScene(string sceneName) {
            var asyncOperation = UnitySceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            await OnDone(asyncOperation);
        }

        private static async Task UnloadScene(string sceneName) {
            var asyncOperation = UnitySceneManager.UnloadSceneAsync(sceneName);

            await OnDone(asyncOperation);
        }

        private static string[] GetAllScenePaths() {
            var sceneCount = UnitySceneManager.sceneCountInBuildSettings;
            var scenePaths = new string[sceneCount];

            for (var i = 0; i < sceneCount; i++) {
                var path = SceneUtility.GetScenePathByBuildIndex(i);
                var startIndex = "Assets/".Length;
                var endIndex = path.Length - ".scene".Length;
                var length = endIndex - startIndex;
                
                scenePaths[i] = path.Substring(startIndex, length);
            }

            return scenePaths;
        }

        private static async Task OnDone(AsyncOperation operation) {
            while (!operation.isDone) {
                await Task.Yield();
            }
        }
    }
}