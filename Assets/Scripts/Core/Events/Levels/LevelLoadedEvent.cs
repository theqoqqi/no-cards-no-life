using Components.Levels;

namespace Core.Events.Levels {
    public class LevelLoadedEvent : IGameEvent {

        public Level Level { get; private set; }

        public void Setup(Level level) {
            Level = level;
        }
    }
}