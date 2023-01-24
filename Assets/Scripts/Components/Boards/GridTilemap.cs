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

        [SerializeField] private TileBase selectedTile;

        private IList<Vector2Int> selectableCells = new List<Vector2Int>();

        private Vector2Int? selectedCell = null;

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

        public void SetSelectedCell(Vector2Int selectedCell) {
            this.selectedCell = selectedCell;
        }

        public void ClearSelectedCell() {
            selectedCell = null;
        }

        private TileBase GetTileForCell(Vector3Int cellPosition) {
            if (!HasCell(cellPosition)) {
                return null;
            }

            if (IsSelectable(cellPosition)) {
                return IsSelected(cellPosition) ? selectedTile : selectableTile;
            }

            return gridTile;
        }

        private bool IsSelectable(Vector3Int cellPosition) {
            return selectableCells.Contains((Vector2Int) cellPosition);
        }

        private bool IsSelected(Vector3Int cellPosition) {
            return selectedCell == (Vector2Int) cellPosition;
        }

        private bool HasCell(Vector3Int cellPosition) {
            return terrainTilemap.IsOnBoard(cellPosition);
        }
    }
}