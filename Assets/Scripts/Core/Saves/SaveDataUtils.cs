using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Cards;
using Core.Cards.Stats;

namespace Core.Saves {
    public static class SaveDataUtils {

        private static readonly IDictionary<Type, Type> CardStatsTypesToCardTypes = new Dictionary<Type, Type> {
                {typeof(MoveCardStats), typeof(MoveCard)},
                {typeof(AttackCardStats), typeof(AttackCard)},
        };
        
        internal static CardStats[] SaveDeck(Deck deck) {
            if (deck == null || deck.Size == 0) {
                return null;
            }
            
            return deck.Cards.Select(card => card.UpcastedStats).ToArray();
        }

        internal static Deck LoadDeck(IReadOnlyCollection<CardStats> saveData) {
            if (saveData == null || saveData.Count == 0) {
                return null;
            }
            
            return new Deck(saveData.Select(CreateCardFromStats));
        }

        private static Card CreateCardFromStats(CardStats stats) {
            var cardStatsType = stats.GetType();
            var cardType = CardStatsTypesToCardTypes[cardStatsType];
            var cardConstructor = GetCardConstructor(cardType, cardStatsType);

            if (cardConstructor == null) {
                throw new Exception(cardType.Name + " must have constructor that accepts " + cardStatsType.Name);
            }

            return (Card) cardConstructor.Invoke(new object[] {stats});
        }

        private static ConstructorInfo GetCardConstructor(Type cardType, Type cardStatsType) {
            const BindingFlags bindingAttrs = BindingFlags.NonPublic
                                              | BindingFlags.Public
                                              | BindingFlags.Instance
                                              | BindingFlags.CreateInstance;

            return cardType.GetConstructor(bindingAttrs, null, new[] {cardStatsType}, null);
        }
    }
}