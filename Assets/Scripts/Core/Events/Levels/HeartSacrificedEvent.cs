namespace Core.Events.Levels {
    public class HeartSacrificedEvent : IGameEvent {
        
        public int HeartIndex { get; private set; }

        public void With(int heartIndex) {
            HeartIndex = heartIndex;
        }
    }
}