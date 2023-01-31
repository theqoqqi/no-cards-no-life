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
            set => starterDeck = value;
        }
        
        private GameRunState currentRun;

        public GameRunState CurrentRun {
            get => currentRun;
            set => currentRun = value;
        }

        public void StartFirstRun() {
            starterDeck = firstRunDeck.Clone();
            
            StartNewRun();
        }

        public void StartNewRun() {
            currentRun = new GameRunState();
            currentRun.Deck = starterDeck.Clone();
        }
    }
}