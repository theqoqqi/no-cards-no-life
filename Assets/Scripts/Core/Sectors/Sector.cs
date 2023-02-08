using System;
using System.Threading.Tasks;
using Components.Locations;

namespace Core.Sectors {
    [Serializable]
    public abstract class Sector {

        public abstract Task Visit(Location location);
    }
}