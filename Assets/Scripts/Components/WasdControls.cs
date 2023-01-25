using Components.Boards;
using Components.Entities;
using Core.Util;
using UnityEngine;

namespace Components {
    public class WasdControls : MonoBehaviour {
        [SerializeField] private GridAlignedBody body;

        private Board board;

        private void Awake() {
            board = GetComponentInParent<Board>();
        }

        private async void Update() {
            if (body.IsMoving) {
                return;
            }

            var h = GetHorizontalDirection();
            var v = GetVerticalDirection();

            if (h.HasValue && CanMoveInDirection(h.Value)) {
                await body.StartMoveTo(h.Value);
            }
            else if (v.HasValue && CanMoveInDirection(v.Value)) {
                await body.StartMoveTo(v.Value);
            }
        }

        private bool CanMoveInDirection(Direction direction) {
            var targetPosition = transform.position.WorldToCell() + direction.AsCellPosition();

            return board.IsCellPassable(targetPosition);
        }

        private Direction? GetHorizontalDirection() {
            var h = Input.GetAxis("Horizontal");

            if (h == 0) {
                return null;
            }

            return h > 0 ? Direction.Right : Direction.Left;

        }

        private Direction? GetVerticalDirection() {
            var v = Input.GetAxis("Vertical");

            if (v == 0) {
                return null;
            }

            return v > 0 ? Direction.Up : Direction.Down;

        }
    }
}