using Components.Combats;
using Components.Entities;

namespace Core.Events.Levels {
    public class TurnEvent : EntityEvent {

        public TurnInfo TurnInfo { get; private set; }

        public void Setup(BaseEntity entity, TurnInfo turnInfo) {
            Setup(entity);

            TurnInfo = turnInfo;
        }
    }
}