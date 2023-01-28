using Components.Combats;

namespace Core.Events.Levels {
    public class CombatEvent : IGameEvent {
        
        public CombatSystem CombatSystem { get; private set; }

        public void Setup(CombatSystem combatSystem) {
            CombatSystem = combatSystem;
        }
    }
}