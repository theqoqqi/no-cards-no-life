using UnityEngine;

namespace Core.Util {
    public static class VectorExtensions {

        private static readonly Vector3 CellOffset = new Vector3(0.5f, 0.5f, 0);

        public static Vector3Int WorldToCell(this Vector3 position) {
            position -= CellOffset;

            return Vector3Int.RoundToInt(position);
        }

        public static Vector3 CellToWorld(this Vector3Int cellPosition) {
            return cellPosition + CellOffset;
        }

        public static Vector3 SnapToCell(this Vector3 position) {
            return position.WorldToCell().CellToWorld();
        }

        public static int ManhattanDistance(this Vector2Int from, Vector2Int to) {
            var dx = Mathf.Abs(from.x - to.x);
            var dy = Mathf.Abs(from.y - to.y);

            return Mathf.Max(dx, dy);
        }
        
        

        public static Vector2Int WorldToCell(this Vector2 position) {
            position -= Vector2.one * 0.5f;

            return Vector2Int.RoundToInt(position);
        }

        public static Vector2 CellToWorld(this Vector2Int cellPosition) {
            return cellPosition + Vector2.one * 0.5f;
        }

        public static Vector2 SnapToCell(this Vector2 position) {
            return position.WorldToCell().CellToWorld();
        }
    }
}