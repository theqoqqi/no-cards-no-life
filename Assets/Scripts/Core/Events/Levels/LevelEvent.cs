using Components.Levels;

namespace Core.Events.Levels {
    public class LevelEvent : IGameEvent {
        
        public Level Level { get; private set; }

        public void With(Level level) {
            Level = level;
        }
    }
}