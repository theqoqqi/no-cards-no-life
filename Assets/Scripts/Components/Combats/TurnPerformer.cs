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
            GameEvents.Instance.Enqueue<TurnPerformStartedEvent>().With(Entity, combatSystem.CurrentTurn);

            await action();

            GameEvents.Instance.Enqueue<TurnPerformFinishedEvent>().With(Entity, combatSystem.CurrentTurn);
        }
    }
}