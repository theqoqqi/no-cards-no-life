using Core.Saves;
using Qoqqi.Inspector.Runtime;
using Udar.SceneManager;
using UnityEngine;

namespace Components.Scenes {
    public class SystemLoader : MonoBehaviour {

        [SerializeField] private SceneField[] dependencies;

        [SerializeField, NullableField] private GameSaveData gameSave;

        private async void Awake() {
            if (Game.IsBootstrapDone) {
                Destroy(gameObject);
                return;
            }

            SceneManager.InitAutoLoad();

            var sceneName = SceneManager.CurrentSceneName;
            var game = await Game.Bootstrap(gameSave.isNull ? null : gameSave);
            var sceneManager = game.SceneManager;

            foreach (var dependency in dependencies) {
                await sceneManager.AutoLoadScene(dependency.Name);
            }
            
            await sceneManager.AutoLoadScene(sceneName);
        }
    }
}