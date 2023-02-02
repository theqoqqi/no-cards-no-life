using Core.Events;
using Core.Events.Levels;

namespace Components.Entities {
    public class Player : BaseEntity {

        protected override void Awake() {
            base.Awake();

            Killed += _ => {
                GameEvents.Instance.Enqueue<PlayerDiedEvent>().With(this);
            };
        }
    }
}