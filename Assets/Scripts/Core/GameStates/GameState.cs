using System;
using Core.Cards;

namespace Core.GameStates {
    public class GameState {

        private Deck currentDeck;

        public Deck CurrentDeck {
            get => currentDeck;
            set {
                if (value == null) {
                    throw new NullReferenceException("Deck can not be null");
                }
                currentDeck = value;
            }
        }
    }
}