using System.Collections.Generic;
using Core.Choices;
using Core.Events;
using Core.Events.Locations;
using UnityEngine;

namespace Components.Locations {
    public class Location : MonoBehaviour {

        private LocationChoiceList choiceList;

        [SerializeField] private LocationMap map;

        public LocationMap Map => map;

        [SerializeField] private ChoiceMetadata[] initialChoices;

        public IList<ChoiceMetadata> InitialChoices => initialChoices;

        private void Awake() {
            choiceList = FindObjectOfType<LocationChoiceList>();
        }

        private void Start() {
            GameEvents.Instance.Enqueue<LocationLoadedEvent>().With(this);
        }

        private void OnEnable() {
            GameEvents.Instance.On<ChoiceSelectedEvent>(OnChoiceSelected);
            GameEvents.Instance.On<SectorEnteredEvent>(OnSectorEntered);
        }

        private void OnDisable() {
            GameEvents.Instance.Off<ChoiceSelectedEvent>(OnChoiceSelected);
            GameEvents.Instance.Off<SectorEnteredEvent>(OnSectorEntered);
        }

        private async void OnChoiceSelected(ChoiceSelectedEvent e) {
            choiceList.SetInteractable(false);

            if (e.Choice.CanBeApplied(this)) {
                e.Button.SetUsed(true);
                await e.Choice.Apply(this);
            }
            
            choiceList.SetInteractable(true);
        }

        private void OnSectorEntered(SectorEnteredEvent e) {
            e.Sector.Visit(this);
        }
    }
}