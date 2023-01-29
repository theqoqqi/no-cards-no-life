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
            Dispatch<TurnFinishedEvent>();
        }

        private async void OnTurnFinished(TurnFinishedEvent e) {
            await Task.Delay(Mathf.FloorToInt(delayBetweenTurns * 1000));
            
            if (!isRunning && combatResult.HasValue) {
                GameEvents.Instance.Dispatch<CombatFinishedEvent>().With(this, combatResult.Value);
                
                return;
            }

            SwitchTurn();

            Dispatch<TurnStartedEvent>();
        }

        public void StartCombat() {
            currentTurn = new TurnInfo(1, 1, 0);
            isRunning = true;
            combatResult = null;
            
            GameEvents.Instance.Dispatch<CombatStartedEvent>().With(this);
            
            Dispatch<TurnStartedEvent>();
        }

        public void FinishCombat(CombatResult result) {
            isRunning = false;
            combatResult = result;
        }
        
        private void Dispatch<T>() where T : TurnEvent, new() {
            GameEvents.Instance.Dispatch<T>().With(CurrentTurnPerformer.Entity, currentTurn.Clone());
        }

        private void SwitchTurn() {
            currentTurn.turn++;
            currentTurn.turnInRound = nextTurnInRound;

            if (currentTurn.turnInRound == 0) {
                currentTurn.round++;
            }
        }
    }
}