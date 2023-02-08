using System;
using Components.Utils;
using Core.Choices;
using UnityEngine;

namespace Components.Locations {
    public class LocationChoiceButton : MonoBehaviour {

        [SerializeField] private SpriteRenderer backgroundSpriteRenderer;

        [SerializeField] private SpriteRenderer actionSpriteRenderer;

        [SerializeField] private Transform topLayerTransform;

        [SerializeField] private Transform bottomLayerTransform;

        private Vector3 pressedOffset;

        private static readonly Color hoveredColor = new Color(0.8f, 0.8f, 0.8f, 1f);

        private static readonly Color idleColor = Color.white;

        private void Awake() {
            pressedOffset = bottomLayerTransform.position - topLayerTransform.position;
            pressedOffset.z = 0;
        }

        public void SetChoice(Choice choice) {
            actionSpriteRenderer.sprite = choice.Sprite;
        }

        public void SetLocalTransformState(Vector3 position, Quaternion rotation, Vector3 scale) {
            transform.SetLocalPositionAndRotation(position, rotation);
            transform.localScale = scale;
        }

        private void OnMouseEnter() {
            backgroundSpriteRenderer.color = hoveredColor;
            actionSpriteRenderer.color = hoveredColor;
        }

        private void OnMouseExit() {
            backgroundSpriteRenderer.color = idleColor;
            actionSpriteRenderer.color = idleColor;
        }

        private void OnMouseDown() {
            topLayerTransform.position += pressedOffset;
        }

        private void OnMouseUp() {
            topLayerTransform.position -= pressedOffset;
        }

        private void OnMouseUpAsButton() {
            Debug.Log("CLICK");
        }
    }
}