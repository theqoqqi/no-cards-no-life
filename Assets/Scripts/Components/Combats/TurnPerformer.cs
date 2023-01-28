using System;
using System.Threading.Tasks;
using Components.Boards;
using Components.Entities;
using Core.Events;
using Core.Events.Levels;
using UnityEngine;

namespace Components.Combats {
    public class TurnPerformer : MonoBehaviour {

        [SerializeField] private BaseEntity entity;

        private Board board;
        
        protected Board Board => board;
        
        private CombatSystem combatSystem;

        public BaseEntity Entity => entity;

        protected virtual void Awake() {
            board = FindObjectOfType<Board>();
            combatSystem = FindObjectOfType<CombatSystem>();
        }

        protected async Task Perform(Func<Task> action) {
            GameEvents.Instance.Dispatch<TurnPerformStartedEvent>(e2 => {
                e2.Setup(Entity, combatSystem.CurrentTurn);
            });

            await action();

            GameEvents.Instance.Dispatch<TurnPerformFinishedEvent>(e2 => {
                e2.Setup(Entity, combatSystem.CurrentTurn);
            });
        }
    }
}