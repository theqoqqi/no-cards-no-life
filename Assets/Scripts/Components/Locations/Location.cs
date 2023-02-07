using Core.Events;
using Core.Events.Locations;
using UnityEngine;

namespace Components.Locations {
    public class Location : MonoBehaviour {

        private void Start() {
            GameEvents.Instance.Enqueue<LocationLoadedEvent>().With(this);
        }
    }
}