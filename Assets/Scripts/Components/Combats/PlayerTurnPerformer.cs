using System.Threading.Tasks;
using Components.Cards;
using Core.Cards;
using Core.Events;
using Core.Events.Cards;
using Core.Events.Levels;
using UnityEngine;

namespace Components.Combats {
    public class PlayerTurnPerformer : TurnPerformer {
        
        private HandRenderer handRenderer;

        protected override void Awake() {
            base.Awake();
            
            handRenderer = FindObjectOfType<HandRenderer>();
        }

        private void OnEnable() {
            GameEvents.Instance.On<TurnStartedEvent>(OnTurnStarted);
            GameEvents.Instance.On<CardReleasedOnSelectableCellEvent>(OnCardReleasedOnSelectableCell);
        }
        private void OnDisable() {
            GameEvents.Instance.Off<TurnStartedEvent>(OnTurnStarted);
            GameEvents.Instance.Off<CardReleasedOnSelectableCellEvent>(OnCardReleasedOnSelectableCell);
        }

        private void OnTurnStarted(TurnStartedEvent e) {
            if (e.Entity != Entity) {
                return;
            }
            
            handRenderer.SetInteractable(true);
        }

        private async void OnCardReleasedOnSelectableCell(CardReleasedOnSelectableCellEvent e) {
            handRenderer.SetInteractable(false);

            await Perform(async () => {
                await UseCard(e.Card, e.CellPosition);
            });
        }

        private async Task UseCard(Card card, Vector3Int cellPosition) {
            Game.Instance.GameState.CurrentHand.RemoveCard(card);

            await card.Use(Board, cellPosition);
            
            GameEvents.Instance.Dispatch<CardUsedEvent>(e => {
                e.Setup(card);
            });
        }
    }
}