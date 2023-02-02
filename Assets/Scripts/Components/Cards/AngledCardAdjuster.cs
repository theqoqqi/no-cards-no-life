using System.Linq;
using UnityEngine;

namespace Components.Cards {
    public class AngledCardAdjuster : CardAdjuster {
        
        [SerializeField] private float radius;

        [SerializeField] private float maxStepDistance;

        [SerializeField] private float maxTotalDistance;

        public override void AdjustCards() {
            var cards = GetComponentsInChildren<CardContainer>()
                    .Where(cc => cc.isActiveAndEnabled)
                    .ToArray();
            var rotations = GetRotations(cards.Length);
            var origin = transform.position + Vector3.down * radius;
            
            for (var i = 0; i < cards.Length; i++) {
                var rotation = rotations[i];
                var card = cards[i];

                var position = GetCardTargetPosition(transform.position, origin, -rotation, i);
                var quaternion = GetCardTargetQuaternion(-rotation);

                card.SetTargetTransformState(position, quaternion, Vector3.one);
            }
        }

        private static Vector3 GetCardTargetPosition(Vector3 from, Vector3 origin, float rotation, int i) {
            var position = RotatePosition(from, origin, rotation);
            
            position.z += i * 0.01f;
            
            return position - from;
        }

        private static Quaternion GetCardTargetQuaternion(float rotation) {
            return Quaternion.Euler(0, 0, rotation);
        }

        private static Vector3 RotatePosition(Vector3 position, Vector3 origin, float rotation) {
            var rotatedDelta = Quaternion.Euler(0, 0, rotation) * (position - origin);
            
            return origin + rotatedDelta;
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
            // Тут должно быть (cardCount - 1), чтобы отступы между картами были одинаковыми при любом количестве карт,
            // но так, как оно работает сейчас, мне нравится даже больше. Чем меньше карт, тем больше между ними отступ.
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