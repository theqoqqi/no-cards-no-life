using System.Collections.Generic;
using Core.Sectors;
using Core.Util;
using UnityEngine;

namespace Components.Locations {
    public class LocationMap : MonoBehaviour {

        [SerializeField] private LocationGrid grid;
        
        private LocationPlayer player;

        public LocationPlayer Player => player;

        private void Awake() {
            player = FindObjectOfType<LocationPlayer>();
        }

        public IList<Vector3Int> GetPathToNextSector(Direction direction) {
            var currentSector = grid.GetSector(player.CellPosition);
            
            return grid.GetPathToNextSector(currentSector, direction);
        }

        public Sector GetSector(Vector3Int cellPosition) {
            var locationSector = grid.GetSector(cellPosition);
            
            return locationSector.Sector;
        }
    }
}