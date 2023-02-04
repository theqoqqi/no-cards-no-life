using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Entities.Parts {
    public class HealthBar : MonoBehaviour {

        [SerializeField] private Health health;

        protected Health Health => health;
        
        [SerializeField] private Heart heartPrefab;

        [SerializeField] private float spacing = 0.25f;

        [SerializeField] private float maxSpace = 1;

        [SerializeField] private Vector3 pivot;

        [SerializeField] private float scale = 1;

        public float Scale => scale;

        private readonly IList<Heart> hearts = new List<Heart>();

        private void Start() {
            if (health) {
                SetHealth(health);
            }
        }

        protected void SetHealth(Health health) {
            if (this.health) {
                this.health.HitPointsChanged.RemoveListener(OnHitPointsChanged);
            }
            
            this.health = health;
            
            if (this.health) {
                this.health.HitPointsChanged.AddListener(OnHitPointsChanged);
                
                StartCoroutine(SetHeartCount(this.health.HitPoints));
            }
        }

        private void OnHitPointsChanged(int changedBy) {
            StartCoroutine(SetHeartCount(health.HitPoints, 0.2f));
        }

        private IEnumerator SetHeartCount(int count, float delayPerHeart = 0f) {
            yield return ModifyHeartCount(count - hearts.Count, delayPerHeart);
        }

        private IEnumerator ModifyHeartCount(int modifyBy, float delayPerHeart) {
            Action action = modifyBy > 0 ? AddHeart : RemoveHeart;
            var absValue = Mathf.Abs(modifyBy);

            if (absValue == 0) {
                Rearrange();
                yield break;
            }

            for (var i = 0; i < absValue; i++) {
                action.Invoke();
                Rearrange();

                yield return new WaitForSeconds(delayPerHeart);
            }
        }

        private void AddHeart() {
            var heart = Instantiate(heartPrefab, transform);

            hearts.Add(heart);
            heart.EasingTransform.TargetLocalScale = Vector3.one * scale;
        }

        private void RemoveHeart() {
            var lastIndex = hearts.Count - 1;

            RemoveHeart(lastIndex);
        }

        protected void RemoveHeart(int index) {
            var heart = hearts[index];

            hearts.RemoveAt(index);
            Destroy(heart.gameObject);
        }

        private void Rearrange() {
            var positions = GetPositions(hearts.Count);

            for (var i = 0; i < hearts.Count; i++) {
                var heart = hearts[i];
                var position = positions[i];

                heart.EasingTransform.TargetLocalPosition = position;
                heart.SetIndex(i);
            }
        }

        private Vector3[] GetPositions(int heartCount) {
            switch (heartCount) {
                case 0: return new Vector3[0];
                case 1: return new[] {Vector3.zero};
            }

            var totalWidth = MathF.Min(maxSpace, spacing * (heartCount - 1));
            var totalHeight = spacing;
            var step = Vector3.right * (totalWidth / (heartCount - 1));
            var firstPosition = new Vector3(pivot.x * -totalWidth, pivot.y * -totalHeight);

            var positions = new Vector3[heartCount];

            for (var i = 0; i < heartCount; i++) {
                positions[i] = (firstPosition + step * i) * scale;
            }

            return positions;
        }
    }
}