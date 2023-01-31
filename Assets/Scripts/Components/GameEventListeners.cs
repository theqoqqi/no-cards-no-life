using Components.Scenes;
using Core.Cards;
using Core.Events;
using Core.Events.Levels;
using UnityEngine;

namespace Components {
    public class GameEventListeners : MonoBehaviour {

        [SerializeField] private Game game;

        [SerializeField] private SceneManager sceneManager;

        private void OnEnable() {
            GameEvents.Instance.On<LevelDoneEvent>(OnLevelDone);
            GameEvents.Instance.On<LevelFailedEvent>(OnLevelFailed);
        }

        private void OnDisable() {
            GameEvents.Instance.Off<LevelDoneEvent>(OnLevelDone);
            GameEvents.Instance.Off<LevelFailedEvent>(OnLevelFailed);
        }

        private async void OnLevelDone(LevelDoneEvent e) {
            var newCard = CreateRandomCard();

            game.GameState.StarterDeck.AddCard(newCard);

            await sceneManager.LoadLocationMap();
        }

        private async void OnLevelFailed(LevelFailedEvent e) {
            var newCard = CreateRandomCard();

            game.GameState.StarterDeck.AddCard(newCard);
            game.GameState.StartNewRun();

            await sceneManager.LoadDeathScreen();
        }

        private static Card CreateRandomCard() {
            if (Random.Range(0, 2) == 0) {
                var maxDistance = Random.Range(1, 4);

                return new MoveCard(maxDistance);
            }
            else {
                var maxDistance = Random.Range(1, 3);
                var damage = Random.Range(1, 3) + Random.Range(0, 2);

                return new AttackCard(maxDistance, damage);
            }
        }
    }
}