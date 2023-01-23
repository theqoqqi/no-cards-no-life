using Components.Utils;
using Core.Cards;
using UnityEngine;

namespace Components.Cards {
    public class CardContainer : MonoBehaviour {

        [SerializeField] private CardController cardController;

        [SerializeField] private EasingTransform easingTransform;

        private void Awake() {
            easingTransform.EasingSpeed = 4f;
        }

        public void SetCard(Card card) {
            cardController.SetCard(card);
        }

        public void SetTargetTransformState(Vector3 position, Quaternion rotation, Vector3 scale) {
            easingTransform.SetTargetState(position, rotation, scale);
        }

        public void SetLocalPositionAndRotation(Vector3 position, Quaternion rotation) {
            transform.SetLocalPositionAndRotation(position, rotation);
            easingTransform.SetTargetState(position, rotation, Vector3.one);
        }
    }
}