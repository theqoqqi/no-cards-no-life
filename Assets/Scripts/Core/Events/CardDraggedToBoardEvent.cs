using Core.Cards;

namespace Core.Events {
    public class CardDraggedToBoardEvent : IGameEvent {

        public Card Card { get; private set; }
        
        public void Setup(Card card) {
            Card = card;
        }
    }
}