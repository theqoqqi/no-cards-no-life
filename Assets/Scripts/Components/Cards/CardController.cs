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

        [SerializeField] private BoxCollider boxCollider;

        [SerializeField] private CardCornerRenderer topLeftCorner;

        [SerializeField] private CardCornerRenderer topRightCorner;

        private SpriteRenderer[] childSpriteRenderers;

        private Card card;

        private HandCamera handCamera;
        
        private float scaleOnBoard = 1f;
        
        private float scaleInHand = 1f;

        private float hoverEasingSpeed = 10f;

        private float dragEasingSpeed = 20f;

        private float useAreaY = 3f;

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

        private void LateUpdate() {
            UpdateCollider();
        }

        private void UpdateDraggedState() {
            if (!isGrabbed) {
                return;
            }
            
            var isMouseInUseArea = IsMouseInUseArea();
            
            if (isDraggedToBoard != isMouseInUseArea) {
                isDraggedToBoard = isMouseInUseArea;

                if (isDraggedToBoard) {
                    GameEvents.Instance.Enqueue<CardDraggedToBoardEvent>().With(card);
                }
                else {
                    GameEvents.Instance.Enqueue<CardDraggedFromBoardEvent>().With(card);
                }
            }
        }

        private void UpdateTargetTransformState() {
            if (isGrabbed) {
                var localMousePosition = GetLocalMousePosition() + grabOffset;

                easingTransform.TargetLocalPosition = new Vector3(
                        localMousePosition.x,
                        localMousePosition.y,
                        transform.localPosition.z
                );
            }
            else if (IsMouseDown) {
                easingTransform.TargetLocalPosition = Vector3.zero;
            }

            var scale = isGrabbed && IsMouseInUseArea() ? scaleOnBoard : scaleInHand;
            
            easingTransform.TargetLocalScale = new Vector3(scale, scale, 1);
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

            ApplyOpacity(actionSpriteRenderer, 1, 1);
        }

        private void UpdateCollider() {
            var y = transform.localPosition.y;
            var center = boxCollider.center;
            var size = boxCollider.size;
            
            center.y = -y / 2;
            size.y = 2 + y;

            boxCollider.center = center;
            boxCollider.size = size;
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
            easingTransform.TargetLocalPosition = Vector3.up;
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
            grabOffset = new Vector3(0, 0.3125f, 0);
            easingTransform.TargetLocalRotation = Quaternion.Inverse(transform.parent.rotation);
            Cursor.visible = false;
        }

        private void OnMouseUp() {
            if (!isInteractable || !isGrabbed) {
                return;
            }
            
            isGrabbed = false;
            easingTransform.TargetLocalPosition = Vector3.zero;
            easingTransform.TargetLocalRotation = Quaternion.identity;
            Cursor.visible = true;

            if (IsMouseInUseArea()) {
                GameEvents.Instance.Enqueue<CardReleasedOnBoardEvent>().With(card);
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
            var bottomPosition = handCamera.Camera.ViewportToWorldPoint(Vector3.zero);
            
            return mousePosition.y >= bottomPosition.y + useAreaY;
        }
    }
}