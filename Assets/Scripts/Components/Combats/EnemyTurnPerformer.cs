using System.Threading.Tasks;
using Components.Entities;
using Core.Events;
using Core.Events.Levels;
using Core.Util;
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
                // TODO: ИИ прописать
                await enemy.Body.StartMoveTo(Direction.Right);
                Debug.Log("Dummy enemy turn performed");
                await Task.CompletedTask;
            });
        }
    }
}