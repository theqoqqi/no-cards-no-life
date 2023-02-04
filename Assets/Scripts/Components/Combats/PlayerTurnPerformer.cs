using System.Threading.Tasks;
using Components.Cards;
using Components.Entities.Parts;
using Core.Cards;
using Core.Events;
using Core.Events.Cards;
using Core.Events.Levels;
using UnityEngine;

namespace Components.Combats {
    public class PlayerTurnPerformer : TurnPerformer {
        
        private HandRenderer handRenderer;
        
        private PlayerHealthBar playerHealthBar;

        protected override void Awake() {
            base.Awake();
            
            handRenderer = FindObjectOfType<HandRenderer>();
            playerHealthBar = FindObjectOfType<PlayerHealthBar>();
        }

        private void OnEnable() {
            GameEvents.Instance.On<TurnStartedEvent>(OnTurnStarted);
            GameEvents.Instance.On<CardReleasedOnSelectableCellEvent>(OnCardReleasedOnSelectableCell);
            GameEvents.Instance.On<HeartSacrificedEvent>(OnHeartSacrificed);
        }
        private void OnDisable() {
            GameEvents.Instance.Off<TurnStartedEvent>(OnTurnStarted);
            GameEvents.Instance.Off<CardReleasedOnSelectableCellEvent>(OnCardReleasedOnSelectableCell);
            GameEvents.Instance.Off<HeartSacrificedEvent>(OnHeartSacrificed);
        }

        private void OnTurnStarted(TurnStartedEvent e) {
            if (e.Entity != Entity) {
                return;
            }
            
            SetControlsInteractable(true);
        }

        private async void OnCardReleasedOnSelectableCell(CardReleasedOnSelectableCellEvent e) {
            SetControlsInteractable(false);

            await Perform(async () => {
                await UseCard(e.Card, e.CellPosition);
            });
        }

        private async void OnHeartSacrificed(HeartSacrificedEvent e) {
            SetControlsInteractable(false);

            await Perform(async () => {
                playerHealthBar.SacrificeHeart(e.HeartIndex);
                Board.Player.Health.ApplyDamage(Board.Player, 1);

                await Task.CompletedTask;
            });
        }

        private async Task UseCard(Card card, Vector3Int cellPosition) {
            Game.Instance.GameState.CurrentRun.Combat.Hand.RemoveCard(card);

            await card.Use(Board, cellPosition);
            
            GameEvents.Instance.Enqueue<CardUsedEvent>().With(card);
        }

        private void SetControlsInteractable(bool isInteractable) {
            handRenderer.SetInteractable(isInteractable);
            playerHealthBar.SetInteractable(isInteractable);
        }
    }
}