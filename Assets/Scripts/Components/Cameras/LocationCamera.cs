using Components.Locations;
using Core.Events;
using Core.Events.Locations;

namespace Components.Cameras {
    public class LocationCamera : AbstractCamera {

        private LocationGrid locationGrid;

        private void OnEnable() {
            GameEvents.Instance.On<LocationLoadedEvent>(OnLocationLoaded);
        }

        private void OnDisable() {
            GameEvents.Instance.Off<LocationLoadedEvent>(OnLocationLoaded);
        }

        private void OnLocationLoaded(LocationLoadedEvent e) {
            locationGrid = FindObjectOfType<LocationGrid>();
            
            SetAtCenterOfGrid();
        }

        private void SetAtCenterOfGrid() {
            Position = locationGrid.Bounds.center;
        }
    }
}