namespace Components.Scenes.Screens {
    public class DeathScreenScene : ScreenScene {
        
        public async void OnClick() {
            await Game.Instance.SceneManager.LoadLocationMap();
        }
    }
}