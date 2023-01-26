using Core.Cards;

namespace Core.Events.Cards {
    public class CardDraggedFromBoardEvent : IGameEvent {

        public Card Card { get; private set; }
        
        public void Setup(Card card) {
            Card = card;
        }
    }
}