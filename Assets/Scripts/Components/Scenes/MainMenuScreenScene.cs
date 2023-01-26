namespace Components.Scenes {
    public class MainMenuScreenScene : ScreenScene {

        public async void OnClickPlay() {
            await Game.Instance.SceneManager.LoadLocationMap();
        }
    }
}