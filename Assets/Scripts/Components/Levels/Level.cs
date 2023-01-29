using System;
using System.Linq;
using Components.Boards;
using Components.Entities;
using Components.Entities.Parts;
using Components.Combats;
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
            
            GameEvents.Instance.Dispatch<LevelLoadedEvent>(e => {
                e.Setup(this);
            });
        }

        private void OnEnable() {
            SetBoard(GetComponentInChildren<Board>());
            
            GameEvents.Instance.On<EntityDestroyedEvent>(OnEntityDestroyed);
            GameEvents.Instance.On<CardUsedEvent>(OnCardUsed);
            GameEvents.Instance.On<CombatFinishedEvent>(OnCombatFinished);
        }

        private void OnDisable() {
            SetBoard(null);
            
            GameEvents.Instance.Off<EntityDestroyedEvent>(OnEntityDestroyed);
            GameEvents.Instance.Off<CardUsedEvent>(OnCardUsed);
            GameEvents.Instance.Off<CombatFinishedEvent>(OnCombatFinished);
        }

        private void OnEntityDestroyed(EntityDestroyedEvent e) {
            if (e.Entity is Enemy && !board.HasEnemies()) {
                combatSystem.FinishCombat(CombatResult.Win);
            }
        }

        private void OnPlayerKilled(Health.DamageDetails damageDetails) {
            GameEvents.Instance.Dispatch<PlayerDiedEvent>(e => {
                e.Setup(this);
            });
            
            combatSystem.FinishCombat(CombatResult.Loose);
        }

        private void OnCardUsed(CardUsedEvent e) {
            var count = Game.Instance.GameState.CurrentHand.Cards.Count();

            if (count == 0) {
                // TODO: это должно быть сделано иначе. Во-первых, я думал сделать кнопку "Умереть",
                //       а во-вторых текущий подход завершит игру, если применить какую-нибудь карту,
                //       которая добавляет еще карту, но при этом эта карта была последней.
                //       Еще нужно не только наличие карт в руке проверять, но и наличие карт в колоде.
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
            GameEvents.Instance.Dispatch<LevelFailedEvent>(e => {
                e.Setup(this);
            });
        }

        private void CompleteLevel() {
            GameEvents.Instance.Dispatch<LevelDoneEvent>(e => {
                e.Setup(this);
            });
        }

        private void SetBoard(Board board) {
            if (this.board) {
                RemoveBoardListeners();
            }
            
            this.board = board;

            if (this.board) {
                AddBoardListeners();
            }
        }

        private void RemoveBoardListeners() {
            board.Player.Killed -= OnPlayerKilled;
        }

        private void AddBoardListeners() {
            board.Player.Killed += OnPlayerKilled;
        }
    }
}