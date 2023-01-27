namespace Components.Scenes.Screens {
    public class MainMenuScreenScene : ScreenScene {

        public async void OnClickPlay() {
            await Game.Instance.SceneManager.LoadLocationMap();
        }
    }
}