using System.Collections.Generic;
using System.Linq;
using Components.Entities;
using UnityEngine;

namespace Components.Boards {
    public class EntityGrid : MonoBehaviour {

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerable<GridAlignedBody> EnumerateBodies() {
            return GetComponentsInChildren<GridAlignedBody>();
        }

        public GridAlignedBody GetBodyAtCell(Vector3Int cellPosition) {
            return EnumerateBodies()
                    .FirstOrDefault(child => child.isActiveAndEnabled && child.CellPosition == cellPosition);
        }

        public GameObject GetObjectAtCell(Vector3Int cellPosition) {
            return GetBodyAtCell(cellPosition)?.gameObject;
        }

        public bool IsCellEmpty(Vector3Int cellPosition) {
            return GetBodyAtCell(cellPosition) == null;
        }

        public bool IsCellPassable(Vector3Int cellPosition) {
            var body = GetBodyAtCell(cellPosition);

            return body == null || body.IsPassable;
        }

        public bool IsCellReserved(Vector3Int cellPosition) {
            return EnumerateBodies()
                    .FirstOrDefault(child => child.isActiveAndEnabled
                                             && child.TargetCellPosition == cellPosition
                                             && child.TargetCellPosition != child.CellPosition);
        }

        public IEnumerable<T> GetObjectsOfType<T>() {
            return GetComponentsInChildren<T>();
        }
    }
}