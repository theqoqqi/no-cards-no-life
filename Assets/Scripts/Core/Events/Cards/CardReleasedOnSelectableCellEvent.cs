using Core.Cards;
using UnityEngine;

namespace Core.Events.Cards {
    public class CardReleasedOnSelectableCellEvent : CardEvent {

        public Vector3Int CellPosition { get; private set; }

        public void Setup(Card card, Vector3Int cellPosition) {
            base.Setup(card);

            CellPosition = cellPosition;
        }
    }
}