using Udar.SceneManager;
using UnityEngine;

namespace Components.Scenes {
    public class SystemLoader : MonoBehaviour {

        [SerializeField] private SceneField[] dependencies;

        private async void Awake() {
            if (Game.IsBootstrapDone) {
                Destroy(gameObject);
                return;
            }

            var sceneName = SceneManager.CurrentSceneName;
            var game = await Game.Bootstrap();
            var sceneManager = game.SceneManager;

            foreach (var dependency in dependencies) {
                await sceneManager.LoadSceneByName(dependency.Name);
            }
            
            await sceneManager.LoadSceneByName(sceneName);
        }
    }
}