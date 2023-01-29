using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Random = UnityEngine.Random;

namespace Core.Cards {
    public class Deck {

        private readonly ObservableCollection<Card> cards;

        public IEnumerable<Card> Cards => cards;

        public event Action<Card> CardAdded;
        
        public event Action<Card> CardRemoved;

        public Deck() : this(new List<Card>()) {

        }

        public Deck(IEnumerable<Card> cards) {
            this.cards = new ObservableCollection<Card>(cards);
            
            this.cards.CollectionChanged += (sender, e) => {
                if (e.Action == NotifyCollectionChangedAction.Add) {
                    foreach (var card in e.NewItems) {
                        CardAdded?.Invoke((Card) card);
                    }
                }
                
                if (e.Action == NotifyCollectionChangedAction.Remove) {
                    foreach (var card in e.OldItems) {
                        CardRemoved?.Invoke((Card) card);
                    }
                }
            };
        }

        public void AddCard(Card card) {
            cards.Add(card);
        }

        public void RemoveCard(Card card) {
            cards.Remove(card);
        }

        public void Shuffle() {
            for (var i = 0; i < cards.Count; i++) {
                var temp = cards[i];
                var randomIndex = Random.Range(i, cards.Count);
                
                cards[i] = cards[randomIndex];
                cards[randomIndex] = temp;
            }
        }
    }
}