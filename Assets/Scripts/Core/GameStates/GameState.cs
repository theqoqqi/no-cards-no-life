using System;
using Core.Cards;

namespace Core.GameStates {
    public class GameState {

        private Deck currentHand;

        public Deck CurrentHand {
            get => currentHand;
            set {
                if (value == null) {
                    throw new NullReferenceException("Deck can not be null");
                }
                currentHand = value;
            }
        }
    }
}