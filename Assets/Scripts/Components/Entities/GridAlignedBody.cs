using System;
using System.Threading.Tasks;
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
        
        private Vector3? targetPosition;
        
        public Vector3Int? TargetCellPosition => targetPosition?.WorldToCell();

        private float currentMoveSpeed = 3f;

        public event Action TargetPositionReached; 

        public bool IsOnTargetPosition => Position == targetPosition;

        public bool IsMoving => targetPosition.HasValue && !IsOnTargetPosition;

        protected override void Awake() {
            base.Awake();
            
            targetPosition = null;
        }

        private void Update() {
            StepTowardsTargetPosition();
        }

        private void StepTowardsTargetPosition() {
            if (!targetPosition.HasValue) {
                return;
            }

            Position = Vector3.MoveTowards(Position, targetPosition.Value, currentMoveSpeed * Time.deltaTime);
            
            if (IsOnTargetPosition) {
                targetPosition = null;
                TargetPositionReached?.Invoke();
            }
        }

        public async Task<Vector3Int> StartMoveTo(Direction direction) {
            return await StartMoveTo(direction, currentMoveSpeed);
        }

        public async Task<Vector3Int> StartMoveTo(Direction direction, float moveSpeed) {
            return await StartMoveTo(Position + direction.AsPosition(), moveSpeed);
        }

        public async Task<Vector3Int> StartMoveTo(Vector3 position) {
            return await StartMoveTo(position, currentMoveSpeed);
        }

        public async Task<Vector3Int> StartMoveTo(Vector3 position, float moveSpeed) {
            StopMovement();

            var taskCompletionSource = new TaskCompletionSource<Vector3Int>();

            targetPosition = position;
            currentMoveSpeed = moveSpeed;

            TargetPositionReached += Listener;

            void Listener() {
                taskCompletionSource.SetResult(Position.WorldToCell());
                TargetPositionReached -= Listener;
            }

            return await taskCompletionSource.Task;
        }
        
        private void StopMovement() {
            var snappedPosition = Position.SnapToCell();

            Position = snappedPosition;
            targetPosition = snappedPosition;
        }
    }
}