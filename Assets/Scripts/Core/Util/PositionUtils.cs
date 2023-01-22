using UnityEngine;

namespace Core.Util {
    public static class PositionUtils {

        public static Vector3Int WorldToCell(this Vector3 position) {
            position -= Vector3.one * 0.5f;

            return Vector3Int.RoundToInt(position);
        }

        public static Vector3 CellToWorld(this Vector3Int cellPosition) {
            return cellPosition + Vector3.one * 0.5f;
        }

        public static Vector3 SnapToCell(this Vector3 position) {
            return position.WorldToCell().CellToWorld();
        }
    }
}