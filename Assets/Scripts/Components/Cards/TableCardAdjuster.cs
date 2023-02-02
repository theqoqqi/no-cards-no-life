using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Components.Cards {
    public class TableCardAdjuster : CardAdjuster {

        [SerializeField] private Vector2 scatterPositionsBy;

        [SerializeField] private float maxStepDistance;

        [SerializeField] private float maxTotalDistance;

        public override void AdjustCards() {
            var cards = GetComponentsInChildren<CardContainer>()
                    .Where(cc => cc.isActiveAndEnabled)
                    .ToArray();
            var positions = GetPositions(cards.Length);

            for (var i = 0; i < cards.Length; i++) {
                var position = positions[i];
                var card = cards[i];

                var randomPosition = GetRandomTargetPosition(position, i);
                var quaternion = GetCardTargetQuaternion();

                card.SetTargetTransformState(randomPosition, quaternion, Vector3.one);
            }
        }

        private Vector3 GetRandomTargetPosition(Vector3 initialPosition, int i) {
            var moveBy = Vector3.zero;

            moveBy.x += scatterPositionsBy.x * (Random.value - Random.value);
            moveBy.y += scatterPositionsBy.y * (Random.value - Random.value);
            moveBy.z += i * 0.01f;

            return initialPosition + moveBy;
        }

        private Quaternion GetCardTargetQuaternion() {
            return Quaternion.identity;
        }

        private Vector3[] GetPositions(int totalCards) {
            switch (totalCards) {
                case 0: return new Vector3[0];
                case 1: return new[] {Vector3.zero};
            }

            var totalWidth = MathF.Min(maxTotalDistance, maxStepDistance * (totalCards - 1));
            var step = Vector3.right * (totalWidth / (totalCards - 1));
            var firstPosition = new Vector3(-totalWidth / 2, 0);

            var positions = new Vector3[totalCards];

            for (var i = 0; i < totalCards; i++) {
                positions[i] = firstPosition + step * i;
            }

            return positions;
        }
    }
}