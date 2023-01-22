using UnityEngine;

namespace Components.Cards {
    public class HandRenderer : MonoBehaviour {

        [SerializeField] private float radius;

        [SerializeField] private float maxStepDistance;

        [SerializeField] private float maxTotalDistance;

        private void Update() {
            AdjustCards();
        }

        private void AdjustCards() {
            var cards = GetComponentsInChildren<CardContainer>();
            var rotations = GetRotations(cards.Length);
            var origin = transform.position + Vector3.down * radius;

            for (var i = 0; i < cards.Length; i++) {
                var rotation = rotations[i];
                var card = cards[i];
                
                card.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                card.transform.RotateAround(origin, Vector3.forward, rotation);
                card.transform.Translate(0, 0, i * -0.01f);
            }
        }

        private float[] GetRotations(int cardCount) {
            if (cardCount == 0) {
                return new float[0];
            }

            if (cardCount == 1) {
                return new[] {0f};
            }

            var maxRotationStep = ChordLengthToAngle(maxStepDistance);
            var maxRotationSpread = ChordLengthToAngle(maxTotalDistance);
            var totalAngle = Mathf.Min(cardCount * maxRotationStep, maxRotationSpread);
            var firstAngle = -totalAngle / 2;
            var rotationStep = totalAngle / (cardCount - 1);
            var rotations = new float[cardCount];
            
            for (var i = 0; i < cardCount; i++) {
                rotations[i] = firstAngle + rotationStep * i;
            }

            return rotations;
        }

        private float ChordLengthToAngle(float length) {
            return length / radius * Mathf.Rad2Deg;
        }
    }
}