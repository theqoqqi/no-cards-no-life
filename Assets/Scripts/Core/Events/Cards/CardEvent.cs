using Core.Cards;

namespace Core.Events.Cards {
    public class CardEvent : IGameEvent {

        public Card Card { get; private set; }
        
        public virtual void Setup(Card card) {
            Card = card;
        }
    }
}