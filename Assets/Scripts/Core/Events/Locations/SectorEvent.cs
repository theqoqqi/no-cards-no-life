using Core.Sectors;

namespace Core.Events.Locations {
    public class SectorEvent : IGameEvent {
        
        public Sector Sector { get; private set; }
        
        public void With(Sector sector) {
            Sector = sector;
        }
    }
}