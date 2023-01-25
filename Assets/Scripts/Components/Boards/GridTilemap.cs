using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Components.Boards {
    public class GridTilemap : MonoBehaviour {

        [SerializeField] private Tilemap tilemap;

        [SerializeField] private TerrainTilemap terrainTilemap;

        [SerializeField] private TileBase gridTile;

        [SerializeField] private TileBase selectableTile;

        [SerializeField] private TileBase hoveredSelectableTile;

        private IList<Vector2Int> selectableCells = new List<Vector2Int>();

        private Vector2Int? hoveredCell = null;

        public void Update() {
            RefillGrid();
        }

        private void RefillGrid() {
            tilemap.ClearAllTiles();

            foreach (var cellPosition in terrainTilemap.CellBounds.allPositionsWithin) {
                var tile = GetTileForCell(cellPosition);

                tilemap.SetTile(cellPosition, tile);
            }
        }

        public void SetSelectableCells(IEnumerable<Vector2Int> selectableCells) {
            this.selectableCells = selectableCells.ToList();
        }

        public void ClearSelectableCells() {
            SetSelectableCells(new Vector2Int[0]);
        }

        public void SetHoveredCell(Vector2Int hoveredCell) {
            this.hoveredCell = hoveredCell;
        }

        public void ClearHoveredCell() {
            hoveredCell = null;
        }

        private TileBase GetTileForCell(Vector3Int cellPosition) {
            if (!HasCell(cellPosition)) {
                return null;
            }

            if (IsSelectable(cellPosition)) {
                return IsHovered(cellPosition) ? hoveredSelectableTile : selectableTile;
            }

            return gridTile;
        }

        private bool IsSelectable(Vector3Int cellPosition) {
            return selectableCells.Contains((Vector2Int) cellPosition);
        }

        private bool IsHovered(Vector3Int cellPosition) {
            return hoveredCell == (Vector2Int) cellPosition;
        }

        private bool HasCell(Vector3Int cellPosition) {
            return terrainTilemap.IsOnBoard(cellPosition);
        }
    }
}