using Core.Events;
using Core.Events.Levels;

namespace Components.Entities.Parts {
    public class PlayerHealthBar : HealthBar {

        private Player player;
        
        private void Awake() {
            TryAttachToPlayer();
        }

        protected void OnEnable() {
            GameEvents.Instance.On<LevelLoadedEvent>(OnLevelLoaded);
        }

        protected void OnDisable() {
            GameEvents.Instance.Off<LevelLoadedEvent>(OnLevelLoaded);
        }

        private void OnLevelLoaded(LevelLoadedEvent e) {
            TryAttachToPlayer();
        }

        private void TryAttachToPlayer() {
            if (Health) {
                return;
            }
            
            player = FindObjectOfType<Player>();

            if (player) {
                SetHealth(player.Health);
            }
        }
    }
}