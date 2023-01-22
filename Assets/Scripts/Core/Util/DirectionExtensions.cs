using System.Collections.Generic;
using UnityEngine;

namespace Core.Util {
    public static class DirectionExtensions {
        private static readonly Dictionary<Direction, Vector3> Positions = new Dictionary<Direction, Vector3> {
                {Direction.Up, Vector3.up},
                {Direction.Down, Vector3.down},
                {Direction.Left, Vector3.left},
                {Direction.Right, Vector3.right},
        };

        private static readonly Dictionary<Direction, Vector3Int> CellPositions = new Dictionary<Direction, Vector3Int> {
                {Direction.Up, Vector3Int.up},
                {Direction.Down, Vector3Int.down},
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