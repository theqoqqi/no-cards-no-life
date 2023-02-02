using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Boards;
using Core.Events;
using Core.Events.Levels;
using UnityEngine;

namespace Components.Combats {
    public class CombatSystem : MonoBehaviour {

        [SerializeField] private Board board;

        [SerializeField] private float delayBetweenTurns = 1;

        private TurnInfo currentTurn;

        public TurnInfo CurrentTurn => currentTurn.Clone();

        private bool isRunning;

        private CombatResult? combatResult;

        private IList<TurnPerformer> turnPerformers => board.TurnPerformers.ToList();
        
        private int nextTurnInRound => (currentTurn.turnInRound + 1) % turnPerformers.Count;
        
        private TurnPerformer CurrentTurnPerformer => turnPerformers[currentTurn.turnInRound];

        private void OnEnable() {
            GameEvents.Instance.On<TurnPerformFinishedEvent>(OnTurnPerformFinished);
            GameEvents.Instance.On<TurnFinishedEvent>(OnTurnFinished);
        }

        private void OnDisable() {
            GameEvents.Instance.Off<TurnPerformFinishedEvent>(OnTurnPerformFinished);
            GameEvents.Instance.Off<TurnFinishedEvent>(OnTurnFinished);
        }

        private void OnTurnPerformFinished(TurnPerformFinishedEvent e) {
            DispatchTurnEvent<TurnFinishedEvent>();
        }

        private async void OnTurnFinished(TurnFinishedEvent e) {
            await Task.Delay(Mathf.FloorToInt(delayBetweenTurns * 1000));
            
            if (!isRunning && combatResult.HasValue) {
                GameEvents.Instance.Enqueue<CombatFinishedEvent>().With(this, combatResult.Value);
                
                return;
            }

            SwitchTurn();

            DispatchTurnEvent<TurnStartedEvent>();
        }

        public void StartCombat() {
            currentTurn = new TurnInfo(1, 1, 0);
            isRunning = true;
            combatResult = null;
            
            GameEvents.Instance.Enqueue<CombatStartedEvent>().With(this);
            
            DispatchTurnEvent<TurnStartedEvent>();
        }

        public void FinishCombat(CombatResult result) {
            isRunning = false;
            combatResult = result;
        }

        private void SwitchTurn() {
            if (nextTurnInRound == 0) {
                DispatchTurnEvent<RoundFinishedEvent>();
            }
            
            currentTurn.turn++;
            currentTurn.turnInRound = nextTurnInRound;

            if (currentTurn.turnInRound == 0) {
                currentTurn.round++;
                DispatchTurnEvent<RoundStartedEvent>();
            }
        }
        
        private void DispatchTurnEvent<T>() where T : TurnEvent, new() {
            GameEvents.Instance.Enqueue<T>().With(CurrentTurnPerformer.Entity, currentTurn.Clone());
        }
    }
}