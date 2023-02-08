using System.Collections.Generic;
using Components.Utils;
using Core.Choices;
using Core.Events;
using Core.Events.Locations;
using UnityEngine;

namespace Components.Locations {
    public class LocationChoiceList : MonoBehaviour {
        
        [SerializeField] protected GameObject choiceContainerPrefab;

        [SerializeField] private AbstractAdjuster adjuster;

        private readonly IList<LocationChoiceButton> ChoiceButtons = new List<LocationChoiceButton>();
        
        private void OnEnable() {
            GameEvents.Instance.On<LocationLoadedEvent>(OnLocationLoaded);
        }

        private void OnDisable() {
            GameEvents.Instance.Off<LocationLoadedEvent>(OnLocationLoaded);
        }

        private void OnLocationLoaded(LocationLoadedEvent e) {
            foreach (var choice in e.Location.InitialChoices) {
                AddChoice(choice);
            }
            
            adjuster.Adjust();
        }
        
        private void AddChoice(Choice choice) {
            AddChoice(choice, Vector3.zero, Quaternion.identity, Vector3.one);
        }
        
        private void AddChoice(Choice choice, Vector3 position, Quaternion rotation, Vector3 scale) {
            var choiceButton = Instantiate(choiceContainerPrefab, gameObject.transform)
                    .GetComponent<LocationChoiceButton>();

            choiceButton.SetLocalTransformState(position, rotation, scale);
            choiceButton.SetChoice(choice);

            ChoiceButtons.Add(choiceButton);
        }

        public void SetInteractable(bool isInteractable) {
            foreach (var button in ChoiceButtons) {
                button.SetInteractable(isInteractable);
            }
        }
    }
}