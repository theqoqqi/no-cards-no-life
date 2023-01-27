using Components.Entities;

namespace Core.Events.Levels {
    public class EntityEvent : IGameEvent {
        
        public BaseEntity Entity { get; private set; }

        public void Setup(BaseEntity entity) {
            Entity = entity;
        }
    }
}