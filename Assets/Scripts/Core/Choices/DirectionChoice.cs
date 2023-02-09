using System.Threading.Tasks;
using Components.Locations;
using Core.Events;
using Core.Events.Locations;
using Core.Util;

namespace Core.Choices {
    public class DirectionChoice : GenericChoice<DirectionChoiceMetadata> {
        
        private Direction Direction => Metadata.Direction;

        public DirectionChoice(DirectionChoiceMetadata metadata) : base(metadata) {
        }
        
        public override bool CanBeApplied(Location location) {
            var path = location.Map.GetPathToNextSector(Direction);

            return path != null && path.Count > 0;
        }

        public override async Task Apply(Location location) {
            var path = location.Map.GetPathToNextSector(Direction);

            foreach (var cellPosition in path) {
                var position = cellPosition.CellToWorld();
                
                await location.Map.Player.Body.StartMoveTo(position);
            }

            var sector = location.Map.GetSector(location.Map.Player.CellPosition);
            
            GameEvents.Instance.Enqueue<SectorEnteredEvent>().With(sector);
        }
    }
}