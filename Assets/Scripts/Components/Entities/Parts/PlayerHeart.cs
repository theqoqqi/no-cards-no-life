using Components.Cameras;
using Components.Combats;
using Core.Events;
using Core.Events.Levels;
using UnityEngine;

namespace Components.Entities.Parts {
    public class PlayerHeart : Heart {

        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private new Collider2D collider;

        private PlayerHealthBar healthBar;
        
        private SkipMovePad skipMovePad;

        private HandCamera handCamera;

        private Vector3 returnPosition;

        private bool isGrabbed;

        private Vector3 grabOffset;

        protected override void Awake() {
            base.Awake();

            handCamera = FindObjectOfType<HandCamera>();
            skipMovePad = FindObjectOfType<SkipMovePad>();
            healthBar = GetComponentInParent<PlayerHealthBar>();
        }

        private void Update() {
            UpdateTargetTransformState();
        }

        private void UpdateTargetTransformState() {
            if (!isGrabbed) {
                return;
            }

            var targetPosition = GetLocalMousePosition() + grabOffset;

            if (skipMovePad.IsMouseOver) {
                targetPosition = skipMovePad.HeartPosition - transform.parent.position;
            }

            EasingTransform.TargetLocalPosition = new Vector3(
                    targetPosition.x,
                    targetPosition.y,
                    transform.localPosition.z
            );

            var scale = skipMovePad.IsMouseOver ? 4 : healthBar.Scale;
            
            EasingTransform.TargetLocalScale = Vector3.one * scale;
        }

        private void OnMouseDown() {
            if (!healthBar.IsInteractable) {
                return;
            }
            
            isGrabbed = true;
            returnPosition = EasingTransform.TargetLocalPosition;
            grabOffset = transform.localPosition - GetLocalMousePosition();
            collider.enabled = false;
        }

        private void OnMouseUp() {
            if (!healthBar.IsInteractable || !isGrabbed) {
                return;
            }
            
            isGrabbed = false;
            EasingTransform.TargetLocalPosition = returnPosition;
            returnPosition = Vector3.zero;
            collider.enabled = true;

            if (skipMovePad.IsMouseOver) {
                GameEvents.Instance.Enqueue<HeartSacrificedEvent>().With(Index);
            }
        }

        private void OnMouseEnter() {
            spriteRenderer.color = new Color(0.75f, 0.25f, 0.25f);
        }

        private void OnMouseExit() {
            spriteRenderer.color = Color.white;
        }

        private Vector3 GetLocalMousePosition() {
            var mousePosition = handCamera.Camera.ScreenToWorldPoint(Input.mousePosition);

            return transform.parent.InverseTransformPoint(mousePosition);
        }
    }
}