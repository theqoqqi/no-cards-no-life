using Core.Cards;

namespace Core.Events.Cards {
    public class CardEvent : IGameEvent {

        public Card Card { get; private set; }
        
        public virtual void With(Card card) {
            Card = card;
        }
    }
}