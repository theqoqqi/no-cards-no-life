using System;
using Core.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

namespace Components.Cards {
    public class CardCornerRenderer : MonoBehaviour {

        [SerializeField] private SpriteRenderer backgroundSpriteRenderer;

        [SerializeField] private SpriteRenderer typeSpriteRenderer;

        [SerializeField] private SpriteRenderer valueSpriteRenderer;

        [SerializeField] private CardDigit cardDigit;

        public void Setup(Card.Corner? corner) {
            gameObject.SetActive(corner.HasValue);
            
            if (!corner.HasValue) {
                return;
            }

            Setup(corner.Value);
        }

        private void Setup(Card.Corner corner) {
            backgroundSpriteRenderer.sprite = corner.BackgroundSprite;
            typeSpriteRenderer.sprite = corner.TypeSprite;
            cardDigit.SetDigit(corner.Value);
        }

        public void SetSortingOrder(int order) {
            valueSpriteRenderer.sortingOrder = order;
        }
    }
}