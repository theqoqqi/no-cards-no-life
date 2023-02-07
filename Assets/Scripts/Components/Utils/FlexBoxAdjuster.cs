using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Components.Utils {
    public class FlexBoxAdjuster : AbstractAdjuster {

        [SerializeField] private Vector3 itemsDirection;
        
        [SerializeField] private float maxStepDistance;

        [SerializeField] private float maxTotalDistance;

        [SerializeField] private Vector2 scatterPositionsBy;

        public override void Adjust() {
            var easingTransforms = GetEasingTransforms();
            var positions = GetPositions(easingTransforms.Length);

            for (var i = 0; i < easingTransforms.Length; i++) {
                var position = positions[i];
                var easingTransform = easingTransforms[i];

                var randomPosition = GetRandomTargetPosition(position, i);
                var quaternion = GetRandomTargetQuaternion();

                easingTransform.SetTargetState(randomPosition, quaternion, Vector3.one);
            }
        }

        private EasingTransform[] GetEasingTransforms() {
            return GetComponentsInChildren<EasingTransform>()
                    .Where(et => et.transform.parent == transform && et.isActiveAndEnabled)
                    .ToArray();
        }

        private Vector3 GetRandomTargetPosition(Vector3 initialPosition, int i) {
            var moveBy = Vector3.zero;

            moveBy.x += scatterPositionsBy.x * (Random.value - Random.value);
            moveBy.y += scatterPositionsBy.y * (Random.value - Random.value);

            return initialPosition + moveBy;
        }

        private Quaternion GetRandomTargetQuaternion() {
            return Quaternion.identity;
        }

        private Vector3[] GetPositions(int totalItems) {
            switch (totalItems) {
                case 0: return new Vector3[0];
                case 1: return new[] {Vector3.zero};
            }

            var totalSpaces = totalItems - 1;
            var totalDistance = MathF.Min(maxTotalDistance, maxStepDistance * totalSpaces);
            var halfDistance = totalDistance / 2;
            var step = itemsDirection * (totalDistance / totalSpaces);
            var firstPosition = new Vector3(itemsDirection.x * -halfDistance, itemsDirection.y * -halfDistance);

            var positions = new Vector3[totalItems];

            for (var i = 0; i < totalItems; i++) {
                positions[i] = firstPosition + step * i;
            }

            return positions;
        }
    }
}