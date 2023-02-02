using System;
using Components.Utils;
using Core.Cards;
using UnityEngine;

namespace Components.Cards {
    public class CardContainer : MonoBehaviour {

        [SerializeField] private CardController cardController;

        public CardController CardController => cardController;

        [SerializeField] private GameObject cardFront;

        [SerializeField] private GameObject cardBack;

        [SerializeField] private EasingTransform easingTransform;

        private void Awake() {
            easingTransform.EasingSpeed = 4f;
        }

        private void Start() {
            UpdateActiveSide();
        }

        private void Update() {
            UpdateActiveSide();
        }

        private void UpdateActiveSide() {
            var isFlipped = transform.localScale.x < 0;

            cardFront.SetActive(!isFlipped);
            cardBack.SetActive(isFlipped);
        }

        public void SetCard(Card card) {
            cardController.SetCard(card);
        }

        public void SetTargetTransformState(Vector3 position, Quaternion rotation, Vector3 scale) {
            easingTransform.SetTargetState(position, rotation, scale);
        }

        public void SetLocalTransformState(Vector3 position, Quaternion rotation, Vector3 scale) {
            transform.SetLocalPositionAndRotation(position, rotation);
            transform.localScale = scale;
            
            SetTargetTransformState(position, rotation, scale);
        }
    }
}