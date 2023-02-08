using Core.Choices;
using Core.Events;
using Core.Events.Locations;
using UnityEngine;

namespace Components.Locations {
    public class LocationChoiceButton : MonoBehaviour {

        [SerializeField] private SpriteRenderer backgroundSpriteRenderer;

        [SerializeField] private SpriteRenderer actionSpriteRenderer;

        [SerializeField] private Transform topLayerTransform;

        [SerializeField] private Transform bottomLayerTransform;

        private Vector3 pressedOffset;

        private Choice choice;

        private bool isHovered;

        private bool isPressed;

        private bool isInteractable = true;

        private bool isUsed;

        private bool IsInteractable => isInteractable && !isUsed;
        
        private static readonly Color usedColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        
        private static readonly Color hoveredColor = new Color(0.8f, 0.8f, 0.8f, 1f);

        private static readonly Color idleColor = Color.white;

        private void Awake() {
            pressedOffset = bottomLayerTransform.position - topLayerTransform.position;
            pressedOffset.z = 0;
        }

        private void Update() {
            UpdateColor();
        }

        private void UpdateColor() {
            if (isUsed) {
                SetColor(usedColor);
                return;
            }
            
            if (isHovered) {
                SetColor(hoveredColor);
                return;
            }
            
            SetColor(idleColor);
        }

        private void OnMouseEnter() {
            isHovered = true;
        }

        private void OnMouseExit() {
            isHovered = false;
        }

        private void OnMouseDown() {
            if (!IsInteractable) {
                return;
            }

            isPressed = true;
            topLayerTransform.position += pressedOffset;
        }

        private void OnMouseUp() {
            if (!IsInteractable || !isPressed) {
                return;
            }

            isPressed = false;
            topLayerTransform.position -= pressedOffset;
        }

        private void OnMouseUpAsButton() {
            if (!IsInteractable) {
                return;
            }
            
            GameEvents.Instance.Enqueue<ChoiceSelectedEvent>().With(choice, this);
        }

        public void SetChoice(Choice choice) {
            this.choice = choice;
            actionSpriteRenderer.sprite = choice.Sprite;
        }

        public void SetLocalTransformState(Vector3 position, Quaternion rotation, Vector3 scale) {
            transform.SetLocalPositionAndRotation(position, rotation);
            transform.localScale = scale;
        }

        public void SetInteractable(bool isInteractable) {
            this.isInteractable = isInteractable;
        }

        public void SetUsed(bool isUsed) {
            this.isUsed = isUsed;
        }

        private void SetColor(Color color) {
            backgroundSpriteRenderer.color = color;
            actionSpriteRenderer.color = color;
        }
    }
}