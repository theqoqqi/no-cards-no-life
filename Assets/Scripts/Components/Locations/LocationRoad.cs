using Core.Util;
using UnityEngine;

namespace Components.Locations {
    public class LocationRoad : LocationCell {

        [SerializeField] private Direction from;
        
        [SerializeField] private Direction to;

        public Vector3Int? GetDestination(Vector3Int cellPosition) {
            var fromCellPosition = CellPosition + from.AsCellPosition();
            var toCellPosition = CellPosition + to.AsCellPosition();

            if (fromCellPosition == cellPosition) {
                return toCellPosition;
            }

            if (toCellPosition == cellPosition) {
                return fromCellPosition;
            }

            return null;
        }
    }
}