using Core.Cards;
using Core.Events;
using Core.Events.Cards;
using Core.Events.Levels;
using UnityEngine;

namespace Components.Cards {
    public class DeckController : AbstractCardSet {
        
        private void OnEnable() {
            GameEvents.Instance.On<LevelLoadedEvent>(OnLevelLoaded);
            GameEvents.Instance.On<CardTakenEvent>(OnCardTaken);
        }

        private void OnDisable() {
            GameEvents.Instance.Off<LevelLoadedEvent>(OnLevelLoaded);
            GameEvents.Instance.Off<CardTakenEvent>(OnCardTaken);
        }

        private void OnLevelLoaded(LevelLoadedEvent e) {
            var cards = Game.Instance.GameState.CurrentRun.Combat.Deck.Cards;

            foreach (var card in cards) {
                AddCard(card);
            }
        }

        private void OnCardTaken(CardTakenEvent e) {
            RemoveCard(e.Card);
        }

        private void AddCard(Card card) {
            var position = RandomPosition(CardContainers.Count);
            var rotation = RandomRotation();
            var scale = new Vector3(-1, 1, 1);
            
            AddCard(card, position, rotation, scale);
        }

        private Vector3 RandomPosition(int cardIndex) {
            return new Vector3(RandomValue(0.2f), RandomValue(0.2f), cardIndex * 0.001f);
        }

        private Quaternion RandomRotation() {
            return Quaternion.Euler(0, 0, 0);
        }

        private float RandomValue(float bound) {
            return (Random.value - Random.value) * bound;
        }
    }
}