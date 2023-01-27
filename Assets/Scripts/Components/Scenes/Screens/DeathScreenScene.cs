namespace Components.Scenes {
    public class DeathScreenScene : ScreenScene {
        
        public async void OnClick() {
            await Game.Instance.SceneManager.LoadLocationMap();
        }
    }
}