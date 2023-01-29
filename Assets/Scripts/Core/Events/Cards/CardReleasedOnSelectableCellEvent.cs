using Core.Cards;
using UnityEngine;

namespace Core.Events.Cards {
    public class CardReleasedOnSelectableCellEvent : CardEvent {

        public Vector3Int CellPosition { get; private set; }

        public void With(Card card, Vector3Int cellPosition) {
            base.With(card);

            CellPosition = cellPosition;
        }
    }
}