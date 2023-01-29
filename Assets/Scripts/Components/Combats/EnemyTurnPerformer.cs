using System.Linq;
using Components.Boards;
using Components.Entities;
using Core.Events;
using Core.Events.Levels;
using Core.Pathfinding;
using UnityEngine;

namespace Components.Combats {
    public class EnemyTurnPerformer : TurnPerformer {

        [SerializeField] private Enemy enemy;
        
        private void OnEnable() {
            GameEvents.Instance.On<TurnStartedEvent>(OnTurnStarted);
        }
        private void OnDisable() {
            GameEvents.Instance.Off<TurnStartedEvent>(OnTurnStarted);
        }

        private async void OnTurnStarted(TurnStartedEvent e) {
            if (e.Entity != enemy) {
                return;
            }

            await Perform(async () => {
                // TODO: Временный ИИ. Нужно переписать, как определюсь с поведением врагов.
                var moveSpeed = 2;
                var attackRange = 1;
                var attackDamage = 1;

                var distanceToPlayer = Board.GetDistanceBetween(enemy, Board.Player);

                if (distanceToPlayer > attackRange) {
                    var findOptions = new AStarSearch.FindOptions();
                    var pathToPlayer = Board.FindPath(enemy, Board.Player, findOptions);
                    var path = pathToPlayer.Take(moveSpeed);

                    foreach (var position in path) {
                        await enemy.Body.StartMoveTo(position);
                    }
                }
                else {
                    await enemy.StartAttack(Board.Player, attackDamage);
                }
            });
        }
    }
}