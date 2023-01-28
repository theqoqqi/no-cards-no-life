using Components.Combats;

namespace Core.Events.Levels {
    public class CombatFinishedEvent : CombatEvent {
        
        public CombatResult CombatResult { get; private set; }

        public void Setup(CombatSystem combatSystem, CombatResult combatResult) {
            Setup(combatSystem);

            CombatResult = combatResult;
        }
    }
}