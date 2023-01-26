using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Components.Scenes {
    public class SceneManager : MonoBehaviour {

        public static string CurrentSceneName => UnitySceneManager.GetActiveScene().name;

        [SerializeField] private ScreenFader screenFader;

        [SerializeField] private Camera fallbackCamera;

        [SerializeField] private float fadeDuration = 1;
        
        private ScreenScene CurrentScreen;
        
        private void Awake() {
            fallbackCamera.enabled = false;
        }

        public async Task LoadLevel() {
            await SwitchScreen("Scenes/Screens/LevelScreen");
        }

        public async Task LoadMainMenu() {
            await SwitchScreen("Scenes/Screens/MainMenuScreen");
        }

        public async Task SwitchScreen(string sceneName) {
            await FadeOut();
            await UnloadCurrentScreen();
            await LoadSceneByName(sceneName);
            await FadeIn();
        }

        public async Task LoadSceneByName(string sceneName) {
            var asyncOperation = UnitySceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!asyncOperation.isDone) {
                await Task.Yield();
            }

            CurrentScreen = FindObjectOfType<ScreenScene>();
        }

        private async Task UnloadCurrentScreen() {
            if (!CurrentScreen) {
                return;
            }
            
            CurrentScreen.Unload();
            
            await Task.CompletedTask;
        }

        private async Task FadeOut() {
            await screenFader.FadeOut(fadeDuration);
            fallbackCamera.enabled = true;
        }

        private async Task FadeIn() {
            fallbackCamera.enabled = false;
            await screenFader.FadeIn(fadeDuration);
        }

        public static void LoadSystemScene() {
            UnitySceneManager.LoadScene("Scenes/SystemScene");
        }
    }
}