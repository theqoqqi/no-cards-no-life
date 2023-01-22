using System;
using Components.Scenes;
using Core.Events;
using UnityEngine;

namespace Components.Cards {
    public class CardController : MonoBehaviour {

        [SerializeField] private SpriteRenderer spriteRenderer;

        private HandCamera handCamera;

        private Vector3 targetLocalPosition;

        private Quaternion targetLocalRotation;

        private Vector3 targetLocalScale;

        private float hoverEasingSpeed = 10f;

        private float dragEasingSpeed = 20f;

        private float useAreaY = -2f;

        private Vector3 grabOffset;

        private bool isHovered;

        private bool isGrabbed;

        private bool IsMouseDown => Input.GetKey(KeyCode.Mouse0);

        private void Awake() {
            targetLocalPosition = transform.localPosition;
            targetLocalRotation = transform.localRotation;
            targetLocalScale = transform.localScale;
            handCamera = FindObjectOfType<HandCamera>();
        }

        private void Update() {
            UpdateTargetTransformState();
            UpdateTransform();
            UpdateSortingOrder();
        }

        private void UpdateTargetTransformState() {
            if (isGrabbed) {
                var localMousePosition = GetLocalMousePosition();

                targetLocalPosition = new Vector3(
                        localMousePosition.x,
                        localMousePosition.y,
                        transform.localPosition.z
                );
            }
            else if (IsMouseDown) {
                targetLocalPosition = Vector3.zero;
            }

            targetLocalScale = Vector3.one * (isGrabbed && IsMouseInUseArea() ? 1f : 1.5f);
        }

        private void UpdateTransform() {
            var easingSpeed = isGrabbed ? dragEasingSpeed : hoverEasingSpeed;

            transform.localPosition = Vector3.Lerp(
                    transform.localPosition,
                    targetLocalPosition,
                    Time.deltaTime * easingSpeed
            );

            transform.localRotation = Quaternion.Lerp(
                    transform.localRotation,
                    targetLocalRotation,
                    Time.deltaTime * easingSpeed
            );

            transform.localScale = Vector3.Lerp(
                    transform.localScale,
                    targetLocalScale,
                    Time.deltaTime * easingSpeed
            );
        }

        private void UpdateSortingOrder() {
            spriteRenderer.sortingOrder = (isHovered && !IsMouseDown) || isGrabbed ? 1 : 0;
        }

        private void OnMouseEnter() {
            isHovered = true;
            targetLocalPosition = Vector3.up * 2;
        }

        private void OnMouseExit() {
            isHovered = false;
            targetLocalPosition = Vector3.zero;
        }

        private void OnMouseDown() {
            isGrabbed = true;
            grabOffset = Vector3.up;
            targetLocalRotation = Quaternion.Inverse(transform.parent.rotation);
        }

        private void OnMouseUp() {
            isGrabbed = false;
            targetLocalPosition = Vector3.zero;
            targetLocalRotation = Quaternion.identity;

            if (IsMouseInUseArea()) {
                GameEvents.Instance.Dispatch<CardUsedEvent>(e => {
                    e.Setup();
                });
            }
        }

        private Vector3 GetLocalMousePosition() {
            var mousePosition = handCamera.Camera.ScreenToWorldPoint(Input.mousePosition);

            return transform.parent.InverseTransformPoint(mousePosition + grabOffset);
        }

        private bool IsMouseInUseArea() {
            var mousePosition = handCamera.Camera.ScreenToWorldPoint(Input.mousePosition);

            return mousePosition.y >= useAreaY;
        }
    }
}