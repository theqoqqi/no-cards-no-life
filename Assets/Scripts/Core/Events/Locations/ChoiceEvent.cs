using Components.Locations;
using Core.Choices;

namespace Core.Events.Locations {
    public class ChoiceEvent : IGameEvent {
        
        public Choice Choice { get; private set; }
        
        public LocationChoiceButton Button { get; private set; }

        public void With(Choice choice, LocationChoiceButton button) {
            Choice = choice;
            Button = button;
        }
    }
}