using Core.Events;
using Core.Events.Levels;

namespace Components.Entities.Parts {
    public class PlayerHealthBar : HealthBar {
        
        private void Awake() {
            SetHealth(FindObjectOfType<Player>()?.Health);
        }

        protected void OnEnable() {
            GameEvents.Instance.On<LevelLoadedEvent>(OnLevelLoaded);
        }

        protected void OnDisable() {
            GameEvents.Instance.Off<LevelLoadedEvent>(OnLevelLoaded);
        }

        private void OnLevelLoaded(LevelLoadedEvent e) {
            if (Health) {
                return;
            }
            
            SetHealth(FindObjectOfType<Player>()?.Health);
        }
    }
}