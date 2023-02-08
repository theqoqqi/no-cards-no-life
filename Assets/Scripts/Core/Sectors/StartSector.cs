using System;
using System.Threading.Tasks;
using Components.Locations;

namespace Core.Sectors {
    [Serializable]
    public class StartSector : Sector {

        public override Task Visit(Location location) {
            return Task.CompletedTask;
        }
    }
}