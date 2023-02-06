using System.Collections.Generic;
using UnityEngine;

namespace Core.Util {
    public static class DirectionExtensions {
        private static readonly Dictionary<Direction, Vector3> Positions = new Dictionary<Direction, Vector3> {
                {Direction.Up, Vector3.up},
                {Direction.UpLeft, Vector3.up + Vector3.left},
                {Direction.UpRight, Vector3.up + Vector3.right},
                {Direction.Down, Vector3.down},
                {Direction.DownLeft, Vector3.down + Vector3.left},
                {Direction.DownRight, Vector3.down + Vector3.right},
                {Direction.Left, Vector3.left},
                {Direction.Right, Vector3.right},
        };

        private static readonly Dictionary<Direction, Vector3Int> CellPositions = new Dictionary<Direction, Vector3Int> {
                {Direction.Up, Vector3Int.up},
                {Direction.UpLeft, Vector3Int.up + Vector3Int.left},
                {Direction.UpRight, Vector3Int.up + Vector3Int.right},
                {Direction.Down, Vector3Int.down},
                {Direction.DownLeft, Vector3Int.down + Vector3Int.left},
                {Direction.DownRight, Vector3Int.down + Vector3Int.right},
                {Direction.Left, Vector3Int.left},
                {Direction.Right, Vector3Int.right},
        };

        public static Vector3 AsPosition(this Direction direction) {
            return Positions[direction];
        }

        public static Vector3Int AsCellPosition(this Direction direction) {
            return CellPositions[direction];
        }
    }
}