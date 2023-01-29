using Core.Cards;

namespace Core.GameStates {
    public class GameRunState {
        
        private Deck deck;

        public Deck Deck {
            get => deck;
            set => deck = Checks.NonNull(value, "CurrentRunDeck can not be null");
        }

        private CombatState combat;

        public CombatState Combat => combat;

        public GameRunState(Deck starterDeck) {
            deck = starterDeck;
        }

        public void StartNewCombat() {
            combat = new CombatState(deck);
        }
    }
}