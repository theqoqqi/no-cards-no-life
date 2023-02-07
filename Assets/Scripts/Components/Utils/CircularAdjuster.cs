using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Components.Utils {
    public class CircularAdjuster : AbstractAdjuster {

        [SerializeField] private float radius;

        [SerializeField] private float maxStepDistance;

        [SerializeField] private float maxTotalDistance;

        public override void Adjust() {
            var easingTransforms = GetComponentsInChildren<EasingTransform>()
                    .Where(et => et.transform.parent == transform && et.isActiveAndEnabled)
                    .ToArray();
            var rotations = GetRotations(easingTransforms.Length);
            var origin = transform.position + Vector3.down * radius;

            for (var i = 0; i < easingTransforms.Length; i++) {
                var rotation = rotations[i];
                var easingTransform = easingTransforms[i];

                var position = GetCardTargetPosition(transform.position, origin, -rotation, i);
                var quaternion = GetCardTargetQuaternion(-rotation);

                easingTransform.SetTargetState(position, quaternion, Vector3.one);
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
            switch (cardCount) {
                case 0: return new float[0];
                case 1: return new[] {0f};
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