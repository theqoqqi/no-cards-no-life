using Core.Cards;

namespace Core.GameStates {
    public class CombatState {
        
        private Deck deck;

        public Deck Deck {
            get => deck;
            set => deck = Checks.NonNull(value, "CurrentCombatDeck can not be null");
        }

        private Deck hand;

        public Deck Hand {
            get => hand;
            set => hand = Checks.NonNull(value, "CurrentHand can not be null");
        }

        public CombatState(Deck initialDeck) {
            deck = initialDeck.Clone();
            hand = new Deck();
            
            deck.Shuffle();
        }

        public void TakeAllCards() {
            while (!deck.IsEmpty) {
                TakeCard();
            }
        }

        public Card TakeCard() {
            var card = deck.TakeCard();
            
            hand.AddCard(card);
            
            return card;
        }
    }
}