using Components.Cameras;
using Components.Utils;
using Core.Cards;
using Core.Events;
using Core.Events.Cards;
using UnityEngine;

namespace Components.Cards {
    public class CardController : MonoBehaviour {

        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private SpriteRenderer actionSpriteRenderer;

        [SerializeField] private EasingTransform easingTransform;

        [SerializeField] private CardCornerRenderer topLeftCorner;

        [SerializeField] private CardCornerRenderer topRightCorner;

        private SpriteRenderer[] childSpriteRenderers;

        private Card card;

        private HandCamera handCamera;

        private float hoverEasingSpeed = 10f;

        private float dragEasingSpeed = 20f;

        private float useAreaY = -2f;

        private Vector3 grabOffset;

        private bool isInteractable = true;

        private bool isHovered;

        private bool isGrabbed;

        private bool isDraggedToBoard;

        private bool IsMouseDown => Input.GetKey(KeyCode.Mouse0);

        private void Awake() {
            card = new MoveCard(0);
            handCamera = FindObjectOfType<HandCamera>();
            childSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }

        private void Update() {
            UpdateDraggedState();
            UpdateTargetTransformState();
            UpdateEasingSpeed();
            UpdateSortingOrder();
            UpdateCardVisibility();
        }

        private void UpdateDraggedState() {
            if (!isGrabbed) {
                return;
            }
            
            var isMouseInUseArea = IsMouseInUseArea();
            
            if (isDraggedToBoard != isMouseInUseArea) {
                isDraggedToBoard = isMouseInUseArea;

                if (isDraggedToBoard) {
                    GameEvents.Instance.Dispatch<CardDraggedToBoardEvent>(e => {
                        e.Setup(card);
                    });
                }
                else {
                    GameEvents.Instance.Dispatch<CardDraggedFromBoardEvent>(e => {
                        e.Setup(card);
                    });
                }
            }
        }

        private void UpdateTargetTransformState() {
            if (isGrabbed) {
                var localMousePosition = GetLocalMousePosition();

                if (!IsMouseInUseArea()) {
                    localMousePosition += grabOffset;
                }

                easingTransform.TargetLocalPosition = new Vector3(
                        localMousePosition.x,
                        localMousePosition.y,
                        transform.localPosition.z
                );
            }
            else if (IsMouseDown) {
                easingTransform.TargetLocalPosition = Vector3.zero;
            }

            easingTransform.TargetLocalScale = Vector3.one * (isGrabbed && IsMouseInUseArea() ? 1f : 1.5f);
        }

        private void UpdateEasingSpeed() {
            easingTransform.EasingSpeed = isGrabbed ? dragEasingSpeed : hoverEasingSpeed;
        }

        private void UpdateSortingOrder() {
            var order = (isHovered && !IsMouseDown) || isGrabbed ? 1 : 0;
            
            spriteRenderer.sortingOrder = order;
            topLeftCorner.SetSortingOrder(order);
            topRightCorner.SetSortingOrder(order);

            foreach (var childSpriteRenderer in childSpriteRenderers) {
                childSpriteRenderer.sortingOrder = order;
            }
        }

        private void UpdateCardVisibility() {
            var targetOpacity = isGrabbed && IsMouseInUseArea() ? 0 : 1;
            
            ApplyOpacity(spriteRenderer, targetOpacity, Time.deltaTime * 10);

            foreach (var childSpriteRenderer in childSpriteRenderers) {
                ApplyOpacity(childSpriteRenderer, targetOpacity, Time.deltaTime * 10);
            }

            topLeftCorner.SetOpacity(targetOpacity);
            topRightCorner.SetOpacity(targetOpacity);

            ApplyOpacity(actionSpriteRenderer, 1, 1);
        }

        private void ApplyOpacity(SpriteRenderer spriteRenderer, int opacity, float lerp) {
            spriteRenderer.color = ApplyOpacity(spriteRenderer.color, opacity, lerp);
        }

        private Color ApplyOpacity(Color color, int opacity, float lerp) {
            color.a = Mathf.Lerp(color.a, opacity, lerp);
            
            return color;
        }

        private void OnMouseEnter() {
            isHovered = true;
            easingTransform.TargetLocalPosition = Vector3.up * 2;
        }

        private void OnMouseExit() {
            isHovered = false;
            easingTransform.TargetLocalPosition = Vector3.zero;
        }

        private void OnMouseDown() {
            if (!isInteractable) {
                return;
            }
            
            isGrabbed = true;
            grabOffset = Vector3.up;
            easingTransform.TargetLocalRotation = Quaternion.Inverse(transform.parent.rotation);
        }

        private void OnMouseUp() {
            if (!isInteractable || !isGrabbed) {
                return;
            }
            
            isGrabbed = false;
            easingTransform.TargetLocalPosition = Vector3.zero;
            easingTransform.TargetLocalRotation = Quaternion.identity;

            if (IsMouseInUseArea()) {
                GameEvents.Instance.Dispatch<CardReleasedOnBoardEvent>(e => {
                    e.Setup(card);
                });
            }
        }

        public void SetCard(Card card) {
            this.card = card;
            
            actionSpriteRenderer.sprite = card.ActionSprite;
            topLeftCorner.Setup(card.TopLeft);
            topRightCorner.Setup(card.TopRight);
        }

        public void SetInteractable(bool isInteractable) {
            this.isInteractable = isInteractable;
        }

        private Vector3 GetLocalMousePosition() {
            var mousePosition = handCamera.Camera.ScreenToWorldPoint(Input.mousePosition);

            return transform.parent.InverseTransformPoint(mousePosition);
        }

        private bool IsMouseInUseArea() {
            var mousePosition = handCamera.Camera.ScreenToWorldPoint(Input.mousePosition);

            return mousePosition.y >= useAreaY;
        }
    }
}