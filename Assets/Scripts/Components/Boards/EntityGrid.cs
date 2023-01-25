using System.Collections.Generic;
using System.Linq;
using Components.Entities;
using UnityEngine;

namespace Components.Boards {
    public class EntityGrid : MonoBehaviour {

        public GridAlignedBody GetBodyAtCell(Vector3Int cellPosition) {
            return GetComponentAtCell<GridAlignedBody>(cellPosition);
        }

        public GameObject GetGameObjectAtCell(Vector3Int cellPosition) {
            return GetBodyAtCell(cellPosition)?.gameObject;
        }

        public T GetObjectAtCell<T>(Vector3Int cellPosition) where T : EntityBehaviour {
            return GetComponentAtCell<T>(cellPosition);
        }

        public bool IsCellEmpty(Vector3Int cellPosition) {
            return GetBodyAtCell(cellPosition) == null;
        }

        public bool IsCellPassable(Vector3Int cellPosition) {
            var body = GetBodyAtCell(cellPosition);

            return body == null || body.IsPassable;
        }

        public IEnumerable<T> GetObjectsOfType<T>() {
            return GetComponentsInChildren<T>();
        }

        private T GetComponentAtCell<T>(Vector3Int cellPosition) where T : EntityBehaviour {
            return Enumerate<T>()
                    .FirstOrDefault(child => child.isActiveAndEnabled && child.CellPosition == cellPosition);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerable<T> Enumerate<T>() where T : EntityBehaviour {
            return GetComponentsInChildren<T>();
        }
    }
}