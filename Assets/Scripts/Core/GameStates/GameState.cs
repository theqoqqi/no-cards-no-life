using Core.Cards;

namespace Core.GameStates {
    public class GameState {

        private Deck firstRunDeck;

        public Deck FirstRunDeck {
            get => firstRunDeck;
            set => firstRunDeck = Checks.NonNull(value, "FirstRunDeck can not be null");
        }

        private Deck starterDeck;

        public Deck StarterDeck {
            get => starterDeck;
            set => starterDeck = Checks.NonNull(value, "StarterDeck can not be null");
        }
        
        private GameRunState currentRun;

        public GameRunState CurrentRun => currentRun;

        public void StartFirstRun() {
            starterDeck = firstRunDeck.Clone();
            
            StartNewRun();
        }

        public void StartNewRun() {
            currentRun = new GameRunState(starterDeck);
        }
    }
}