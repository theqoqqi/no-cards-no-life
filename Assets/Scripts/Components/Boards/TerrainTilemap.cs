using UnityEngine;
using UnityEngine.Tilemaps;

namespace Components.Boards {
    public class TerrainTilemap : MonoBehaviour {
        
        [SerializeField] private Tilemap tilemap;

        public Vector3 CenterPosition => tilemap.cellBounds.center;
        
        public int Height => tilemap.size.y;

        public BoundsInt CellBounds => tilemap.cellBounds;

        private void Awake() {
            tilemap.CompressBounds();
        }

        public bool IsOnBoard(Vector3Int cellPosition) {
            return tilemap.HasTile((Vector3Int) (Vector2Int) cellPosition);
        }

        public bool IsCellPassable(Vector3Int cellPosition) {
            return IsOnBoard(cellPosition);
        }
    }
}