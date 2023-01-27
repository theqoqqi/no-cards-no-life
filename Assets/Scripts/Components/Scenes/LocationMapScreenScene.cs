using Udar.SceneManager;
using UnityEngine;

namespace Components.Scenes {
    public class LocationMapScreenScene : ScreenScene {

        [SerializeField] private SceneField testLevelScene;

        public async void OnClick() {
            await Game.Instance.SceneManager.LoadLevel(testLevelScene.Name);
        }
    }
}