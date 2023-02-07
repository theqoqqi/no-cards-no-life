using System.Collections.Generic;
using Core.Util;
using UnityEngine;

namespace Components.Locations {
    internal class LocationCellWalker {

        private readonly LocationGrid grid;

        private IList<Vector3Int> path;

        private Vector3Int current;

        private Vector3Int step;

        public IList<Vector3Int> Path => new List<Vector3Int>(path);

        public LocationCellWalker(LocationGrid grid) {
            this.grid = grid;
        }

        public bool Walk(LocationSector from, Direction direction) {
            Setup(from, direction);
            WalkByRoad();

            return grid.HasSector(current);
        }

        private void Setup(LocationSector from, Direction direction) {
            path = new List<Vector3Int>();
            step = direction.AsCellPosition();
            current = from.CellPosition;
        }

        private void WalkByRoad() {
            DoStep();

            while (grid.HasRoad(current)) {
                var prev = current - step;
                var road = grid.GetRoad(current);
                var roadDestination = road.GetDestination(prev);

                if (!roadDestination.HasValue) {
                    return;
                }
                    
                DoStep();
            }
        }

        private void DoStep() {
            current += step;
                
            path.Add(current);
        }
    }
}