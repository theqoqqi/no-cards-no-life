using System.Collections.Generic;
using Core.Util;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Components.Locations {
    public class LocationGrid : MonoBehaviour {

        [SerializeField] private Tilemap tilemap;

        private readonly IDictionary<Vector3Int, LocationCell> cells = new Dictionary<Vector3Int, LocationCell>();

        private LocationCellWalker cellWalker;

        public BoundsInt Bounds => tilemap.cellBounds;

        private void Awake() {
            cellWalker = new LocationCellWalker(this);
            
            tilemap.CompressBounds();
            CollectCells();
        }

        private void CollectCells() {
            cells.Clear();

            var foundCells = GetComponentsInChildren<LocationCell>();

            foreach (var cell in foundCells) {
                cells[cell.CellPosition] = cell;
            }
        }

        public IList<Vector3Int> GetPathToNextSector(LocationSector origin, Direction direction) {
            if (!cellWalker.Walk(origin, direction)) {
                return null;
            }

            return cellWalker.Path;
        }

        internal LocationRoad GetRoad(Vector3Int cellPosition) {
            return GetCell<LocationRoad>(cellPosition);
        }

        internal bool HasRoad(Vector3Int cellPosition) {
            return HasCell<LocationRoad>(cellPosition);
        }

        private LocationSector GetSector(Vector3Int cellPosition) {
            return GetCell<LocationSector>(cellPosition);
        }

        internal bool HasSector(Vector3Int cellPosition) {
            return HasCell<LocationSector>(cellPosition);
        }

        private T GetCell<T>(Vector3Int cellPosition) where T : LocationCell {
            if (!HasCell<T>(cellPosition)) {
                return null;
            }

            return (T) cells[cellPosition];
        }

        private bool HasCell<T>(Vector3Int cellPosition) where T : LocationCell {
            return cells.ContainsKey(cellPosition) && cells[cellPosition] is T;
        }

    }

}