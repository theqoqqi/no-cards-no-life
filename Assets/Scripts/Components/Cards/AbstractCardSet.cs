using System.Collections.Generic;
using Core.Cards;
using UnityEngine;

namespace Components.Cards {
    public abstract class AbstractCardSet : MonoBehaviour {
        
        [SerializeField] protected GameObject cardContainerPrefab;

        protected readonly Dictionary<Card, CardContainer> CardContainers = new Dictionary<Card, CardContainer>();

        protected void AddCard(Card card, Vector3 position, Quaternion rotation, Vector3 scale) {
            var cardContainer = Instantiate(cardContainerPrefab, gameObject.transform)
                    .GetComponent<CardContainer>();

            cardContainer.SetLocalTransformState(position, rotation, scale);
            cardContainer.SetCard(card);

            CardContainers[card] = cardContainer;
        }

        protected void RemoveCard(Card card) {
            var cardContainer = CardContainers[card];

            CardContainers.Remove(card);
            Destroy(cardContainer.gameObject);
        }
    }
}