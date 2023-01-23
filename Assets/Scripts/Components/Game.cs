using Core.Cards;
using Core.Events;
using Core.GameStates;
using UnityEngine;

namespace Components {
    public class Game : MonoBehaviour {

        public static Game Instance { get; private set; }

        public GameState GameState { get; } = new GameState();

        private void Awake() {
            Instance = this;
            
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            
            

            var deck = new Deck();

            deck.AddCard(new MoveCard(1));
            deck.AddCard(new MoveCard(2));
            deck.AddCard(new MoveCard(3));
            deck.AddCard(new AttackCard(2, 1));
            deck.AddCard(new AttackCard(1, 2));
            
            // deck.Shuffle();
            
            GameState.CurrentDeck = deck;
            
            
            
            GameEvents.Instance.On<CardUsedEvent>(e => {
                GameState.CurrentDeck.RemoveCard(e.Card);
            });
        }
    }
}