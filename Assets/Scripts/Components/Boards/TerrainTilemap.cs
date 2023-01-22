using UnityEngine;
using UnityEngine.Tilemaps;

namespace Components.Boards {
    public class TerrainTilemap : MonoBehaviour {
        
        private Tilemap tilemap;

        public Vector3 CenterPosition => tilemap.cellBounds.center;
        
        public int Height => tilemap.size.y;

        private void Awake() {
            tilemap = GetComponentInChildren<Tilemap>();
            
            tilemap.CompressBounds();
        }

        public bool IsOnBoard(Vector3Int cellPosition) {
            return tilemap.HasTile(cellPosition);
        }

        public bool IsCellPassable(Vector3Int cellPosition) {
            return IsOnBoard(cellPosition);
        }
    }
}