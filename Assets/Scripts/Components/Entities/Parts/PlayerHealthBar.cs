using Core.Events;
using Core.Events.Levels;

namespace Components.Entities.Parts {
    public class PlayerHealthBar : HealthBar {
        
        private void Awake() {
            health = FindObjectOfType<Player>()?.Health;
        }

        protected override void OnEnable() {
            base.OnEnable();
            
            GameEvents.Instance.On<LevelLoadedEvent>(OnLevelLoaded);
        }

        protected override void OnDisable() {
            base.OnDisable();
            
            GameEvents.Instance.Off<LevelLoadedEvent>(OnLevelLoaded);
        }

        private void OnLevelLoaded(LevelLoadedEvent e) {
            if (health) {
                return;
            }
            
            health = FindObjectOfType<Player>()?.Health;

            if (health) {
                StartCoroutine(SetHeartCount(health.HitPoints));
            }
        }
    }
}