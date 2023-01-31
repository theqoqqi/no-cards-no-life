using Core.Cards;

namespace Core.GameStates {
    public class GameRunState {
        
        private Deck deck;

        public Deck Deck {
            get => deck;
            set => deck = value;
        }

        private CombatState combat;

        public CombatState Combat => combat;

        public void StartNewCombat() {
            combat = new CombatState(deck);
        }
    }
}