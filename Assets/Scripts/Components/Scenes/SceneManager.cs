using System.Threading.Tasks;
using Components.Scenes.Screens;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Components.Scenes {
    public class SceneManager : MonoBehaviour {

        public static string CurrentSceneName => UnitySceneManager.GetActiveScene().name;

        [SerializeField] private ScreenFader screenFader;

        [SerializeField] private Camera fallbackCamera;

        [SerializeField] private float fadeDuration = 1;
        
        private ScreenScene currentScreen;

        private string currentScreenSceneName;
        
        private void Awake() {
            fallbackCamera.enabled = false;
        }

        public async Task LoadMainMenu() {
            await SwitchScreen("Scenes/Screens/MainMenuScreen");
        }

        public async Task LoadLocationMap() {
            await SwitchScreen("Scenes/Screens/LocationMapScreen");
        }

        public async Task LoadLevel(string levelSceneName) {
            await SwitchScreen("Scenes/Screens/LevelScreen", levelSceneName);
        }

        public async Task LoadDeathScreen() {
            await SwitchScreen("Scenes/Screens/DeathScreen");
        }

        private async Task SwitchScreen(string sceneName) {
            await SwitchScreen(sceneName, null);
        }

        private async Task SwitchScreen(string sceneName, string contentSceneName) {
            await FadeOut();
            await UnloadCurrentScreen();
            await LoadSceneByName(sceneName);
            await LoadContentScene(contentSceneName);
            await FadeIn();
        }

        public async Task LoadSceneByName(string sceneName) {
            var asyncOperation = UnitySceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            await OnDone(asyncOperation);

            currentScreen = FindObjectOfType<ScreenScene>();
            currentScreenSceneName = sceneName;
        }

        private async Task LoadContentScene(string contentSceneName) {
            if (contentSceneName != null) {
                await LoadSceneByName(contentSceneName);
            }
        }

        private async Task UnloadCurrentScreen() {
            if (!currentScreen) {
                return;
            }
            
            currentScreen.Unload();
            
            var asyncOperation = UnitySceneManager.UnloadSceneAsync(currentScreenSceneName);

            await OnDone(asyncOperation);
            
            currentScreenSceneName = null;
        }

        private async Task FadeOut() {
            await screenFader.FadeOut(fadeDuration);
            fallbackCamera.enabled = true;
        }

        private async Task FadeIn() {
            fallbackCamera.enabled = false;
            await screenFader.FadeIn(fadeDuration);
        }

        private static async Task OnDone(AsyncOperation operation) {
            while (!operation.isDone) {
                await Task.Yield();
            }
        }

        public static void LoadSystemScene() {
            UnitySceneManager.LoadScene("Scenes/SystemScene");
        }
    }
}