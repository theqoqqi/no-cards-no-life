using System.Collections.Generic;
using System.Linq;
using Components.Boards;
using UnityEngine;

namespace Core.Pathfinding {
    public class BoardPassabilityGrid {
        private static readonly Vector2Int[] NeighbourOffsets = {
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, -1)
        };

        private static readonly Vector2Int[] AdjacentOffsets = {
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1)
        };

        private readonly Board board;

        public BoardPassabilityGrid(Board board) {
            this.board = board;
        }

        public bool IsInBounds(Vector2Int position) {
            return board.IsOnBoard((Vector3Int) position);
        }

        public bool IsDirectlyPassable(Vector2Int position) {
            return IsCellPassable(position) && GetTileCost(position) > 0;
        }

        private bool IsCrossPassable(Vector2Int position) {
            return IsCellPassable(position);
        }

        private bool IsCellPassable(Vector2Int position) {
            return IsInBounds(position) && board.IsCellPassable((Vector3Int) position);
        }

        private bool CanCrossPass(Vector2Int from, Vector2Int to) {
            if (from.x == to.x || from.y == to.y) {
                return true;
            }

            var diff = to - from;
            var crossedByX = from + new Vector2Int(diff.x, 0);
            var crossedByY = from + new Vector2Int(0, diff.y);

            return IsCrossPassable(crossedByX) && IsCrossPassable(crossedByY);
        }

        private float GetTileCost(Vector2Int position) {
            var passable = board.IsCellPassable((Vector3Int) position);

            return passable ? 1 : 0;
        }

        public float Cost(Vector2Int a, Vector2Int b) {
            var cost = GetTileCost(a) / 2 + GetTileCost(b) / 2;

            return cost * AStarSearch.Heuristic(a, b);
        }

        public IEnumerable<Vector2Int> GetAvailableAdjacents(Vector2Int position) {
            return GetAvailableNeighbors(position, false);
        }

        public IEnumerable<Vector2Int> GetAvailableNeighbors(Vector2Int position) {
            return GetAvailableNeighbors(position, true);
        }

        private IEnumerable<Vector2Int> GetAvailableNeighbors(Vector2Int position, bool allowDiagonal) {
            return (allowDiagonal ? NeighbourOffsets : AdjacentOffsets)
                .Select(dir => new Vector2Int(position.x + dir.x, position.y + dir.y))
                .Where(next => IsInBounds(next) && IsDirectlyPassable(next) && CanCrossPass(position, next));
        }

        public bool IsFreeOfIdleObjects(Vector2Int position) {
            return !board.HasBodyAtCell((Vector3Int) position);
        }

        public bool IsUsedAsGoal(Vector2Int cell) {
            return false;
        }
    }
}