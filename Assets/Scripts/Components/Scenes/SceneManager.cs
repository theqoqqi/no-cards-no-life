using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Components.Scenes {
    public class SceneManager : MonoBehaviour {

        public static string CurrentSceneName => UnitySceneManager.GetActiveScene().name;
        
        private void Awake() {
        }

        public async Task LoadLevel() {
            await LoadSceneByName("Scenes/Screens/LevelScene");
        }

        public async Task LoadMainMenu() {
            await LoadSceneByName("Scenes/Screens/MainMenuScene");
        }

        public async Task LoadSceneByName(string sceneName) {
            var asyncOperation = UnitySceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!asyncOperation.isDone) {
                await Task.Yield();
            }
        }

        public static void LoadSystemScene() {
            UnitySceneManager.LoadScene("Scenes/SystemScene");
        }
    }
}