using System.Threading.Tasks;
using Components.Locations;
using Core.Util;
using UnityEngine;

namespace Core.Choices {
    [CreateAssetMenu(fileName = "Direction Choice", menuName = "NCNL/Choices/Direction Choice", order = 0)]
    public class DirectionChoice : Choice {

        [SerializeField] private Direction direction;

        public Direction Direction => direction;

        public override bool CanBeApplied(Location location) {
            var path = location.Map.GetPathToNextSector(direction);

            return path != null && path.Count > 0;
        }

        public override async Task Apply(Location location) {
            var path = location.Map.GetPathToNextSector(direction);

            foreach (var cellPosition in path) {
                var position = cellPosition.CellToWorld();
                
                await location.Map.Player.Body.StartMoveTo(position);
            }
        }
    }
}