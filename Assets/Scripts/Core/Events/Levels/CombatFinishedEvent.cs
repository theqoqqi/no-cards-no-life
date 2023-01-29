using Components.Combats;

namespace Core.Events.Levels {
    public class CombatFinishedEvent : CombatEvent {
        
        public CombatResult CombatResult { get; private set; }

        public void With(CombatSystem combatSystem, CombatResult combatResult) {
            With(combatSystem);

            CombatResult = combatResult;
        }
    }
}