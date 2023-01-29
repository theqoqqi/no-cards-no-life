using Components.Combats;
using Components.Entities;

namespace Core.Events.Levels {
    public class TurnEvent : EntityEvent {

        public TurnInfo TurnInfo { get; private set; }

        public void With(BaseEntity entity, TurnInfo turnInfo) {
            With(entity);

            TurnInfo = turnInfo;
        }
    }
}