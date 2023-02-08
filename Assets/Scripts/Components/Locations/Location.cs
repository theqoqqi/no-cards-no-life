using System.Collections.Generic;
using Core.Choices;
using Core.Events;
using Core.Events.Locations;
using UnityEngine;

namespace Components.Locations {
    public class Location : MonoBehaviour {

        [SerializeField] private Choice[] initialChoices;

        public IList<Choice> InitialChoices => initialChoices;
        
        private void Start() {
            GameEvents.Instance.Enqueue<LocationLoadedEvent>().With(this);
        }
    }
}