namespace Components.Entities.Parts {
    public class PlayerHealthBar : HealthBar {

        private void Awake() {
            health = FindObjectOfType<Player>()?.Health;
        }
    }
}