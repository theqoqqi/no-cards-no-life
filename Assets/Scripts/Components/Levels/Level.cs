using System;
using System.Collections;
using Components.Boards;
using Components.Combats;
using Components.Entities;
using Core.Events;
using Core.Events.Cards;
using Core.Events.Levels;
using UnityEngine;

namespace Components.Levels {
    public class Level : MonoBehaviour {

        private Board board;

        [SerializeField] private CombatSystem combatSystem;
        
        private void Start() {
            combatSystem.StartCombat();
            Game.Instance.GameState.CurrentRun.StartNewCombat();

            GameEvents.Instance.Enqueue<LevelLoadedEvent>().With(this);

            StartCoroutine(TakeFirstCards());
        }

        private static IEnumerator TakeFirstCards() {
            var totalCards = Game.Instance.GameState.CurrentRun.Combat.Deck.Size;
            var cardsToTake = Mathf.Min(totalCards, 3);
            var takeCardDelay = new WaitForSeconds(0.8f / cardsToTake);

            for (var i = 0; i < cardsToTake; i++) {
                yield return takeCardDelay;
                
                var card = Game.Instance.GameState.CurrentRun.Combat.TakeCard();

                GameEvents.Instance.Enqueue<CardTakenEvent>().With(card);
            }
        }

        private void OnDestroy() {
            StopAllCoroutines();
        }

        private void OnEnable() {
            board = GetComponentInChildren<Board>();
            
            GameEvents.Instance.On<EntityDestroyedEvent>(OnEntityDestroyed);
            GameEvents.Instance.On<PlayerDiedEvent>(OnPlayerKilled);
            GameEvents.Instance.On<CardUsedEvent>(OnCardUsed);
            GameEvents.Instance.On<CombatFinishedEvent>(OnCombatFinished);
        }

        private void OnDisable() {
            board = null;
            
            GameEvents.Instance.Off<EntityDestroyedEvent>(OnEntityDestroyed);
            GameEvents.Instance.Off<PlayerDiedEvent>(OnPlayerKilled);
            GameEvents.Instance.Off<CardUsedEvent>(OnCardUsed);
            GameEvents.Instance.Off<CombatFinishedEvent>(OnCombatFinished);
        }

        private void OnEntityDestroyed(EntityDestroyedEvent e) {
            if (e.Entity is Enemy && !board.HasEnemies()) {
                combatSystem.FinishCombat(CombatResult.Win);
            }
        }

        private void OnPlayerKilled(PlayerDiedEvent e) {
            combatSystem.FinishCombat(CombatResult.Loose);
        }

        private void OnCardUsed(CardUsedEvent e) {
            var handSize = Game.Instance.GameState.CurrentRun.Combat.Hand.Size;
            var deckSize = Game.Instance.GameState.CurrentRun.Combat.Deck.Size;

            if (handSize == 0 && deckSize == 0) {
                combatSystem.FinishCombat(CombatResult.Loose);
            }
        }

        private void OnCombatFinished(CombatFinishedEvent e) {
            if (e.CombatResult == CombatResult.Win) {
                CompleteLevel();
            }
            else if (e.CombatResult == CombatResult.Loose) {
                GameOver();
            }
            else {
                throw new ArgumentOutOfRangeException("Unknown combat result: " + e.CombatResult);
            }
        }

        private void GameOver() {
            GameEvents.Instance.Enqueue<LevelFailedEvent>().With(this);
        }

        private void CompleteLevel() {
            GameEvents.Instance.Enqueue<LevelDoneEvent>().With(this);
        }
    }
}