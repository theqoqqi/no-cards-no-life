using System.Collections.Generic;
using System.Linq;
using Core.Cards;
using Core.Events;
using Core.Events.Cards;
using UnityEngine;

namespace Components.Cards {
    public class HandRenderer : AbstractCardSet {

        [SerializeField] private Transform deckTransform;

        [SerializeField] private CardAdjuster cardAdjuster;
        
        private IEnumerable<CardController> CardControllers => CardContainers.Values.Select(cc => cc.CardController);

        private void OnEnable() {
            GameEvents.Instance.On<CardTakenEvent>(OnCardTaken);
            GameEvents.Instance.On<CardReleasedOnSelectableCellEvent>(OnCardReleasedOnSelectableCell);
        }

        private void OnDisable() {
            GameEvents.Instance.Off<CardTakenEvent>(OnCardTaken);
            GameEvents.Instance.Off<CardReleasedOnSelectableCellEvent>(OnCardReleasedOnSelectableCell);
        }

        private void OnCardTaken(CardTakenEvent e) {
            AddCard(e.Card);
            cardAdjuster.AdjustCards();
        }

        private void OnCardReleasedOnSelectableCell(CardReleasedOnSelectableCellEvent e) {
            RemoveCard(e.Card);
            cardAdjuster.AdjustCards();
        }

        public void SetInteractable(bool isInteractable) {
            foreach (var cardController in CardControllers) {
                cardController.SetInteractable(isInteractable);
            }
        }

        private void AddCard(Card card) {
            var position = deckTransform.position - transform.position;
            var rotation = Quaternion.identity;
            var scale = new Vector3(-1, 1, 1);
            
            AddCard(card, position, rotation, scale);
        }
    }
}