using System;
using Core.Util;
using UnityEngine;

namespace Components.Entities {
    public class GridAlignedBody : EntityBehaviour {

        [SerializeField] private bool isPassable;

        public bool IsPassable => isPassable;

        private Vector3 Position {
            get => transform.position;
            set => transform.position = value;
        }
        
        private Vector3 targetPosition;
        
        public Vector3Int TargetCellPosition => targetPosition.WorldToCell();

        private float currentMoveSpeed = 3f;

        public event Action OnTargetPositionReached; 

        public bool IsOnTargetPosition => Position == targetPosition;

        public bool IsMoving => !IsOnTargetPosition;

        protected override void Awake() {
            base.Awake();
            
            targetPosition = Position;
        }

        private void Update() {
            StepTowardsTargetPosition();
        }

        private void StepTowardsTargetPosition() {
            if (IsOnTargetPosition) {
                return;
            }
            
            Position = Vector3.MoveTowards(Position, targetPosition, currentMoveSpeed * Time.deltaTime);
            
            if (IsOnTargetPosition) {
                OnTargetPositionReached?.Invoke();
            }
        }

        public void StartMoveTo(Direction direction) {
            StartMoveTo(direction, currentMoveSpeed);
        }

        public void StartMoveTo(Direction direction, float moveSpeed) {
            StopMovement();
            
            targetPosition = Position + direction.AsPosition();
            currentMoveSpeed = moveSpeed;
        }
        
        private void StopMovement() {
            var snappedPosition = Position.SnapToCell();

            Position = snappedPosition;
            targetPosition = snappedPosition;
        }
    }
}