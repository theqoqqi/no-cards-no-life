using Components.Locations;

namespace Core.Events.Locations {
    public class LocationEvent : IGameEvent {
        
        public Location Location { get; private set; }

        public void With(Location location) {
            Location = location;
        }
    }
}