using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Cards;
using Core.Cards.Stats;
using Core.GameStates;

namespace Core.Saves {
    public static class SaveLoadSystem {

        private static readonly IDictionary<Type, Type> CardStatsTypesToCardTypes = new Dictionary<Type, Type> {
                {typeof(MoveCardStats), typeof(MoveCard)},
                {typeof(AttackCardStats), typeof(AttackCard)},
        };

        public static GameSaveData Save(GameState gameState) {
            var saveData = new GameSaveData();

            saveData.firstRunDeck = SaveDeck(gameState.FirstRunDeck);
            saveData.starterDeck = SaveDeck(gameState.StarterDeck);
            saveData.currentRun = SaveGameRun(gameState.CurrentRun);

            return saveData;
        }

        public static GameState Load(GameSaveData saveData) {
            var gameState = new GameState();

            gameState.FirstRunDeck = LoadDeck(saveData.firstRunDeck);
            gameState.StarterDeck = LoadDeck(saveData.starterDeck);
            gameState.CurrentRun = LoadGameRun(saveData.currentRun);

            return Repair(gameState);
        }

        private static GameState Repair(GameState gameState) {
            if (gameState.StarterDeck == null) {
                gameState.StartFirstRun();
            }

            if (gameState.CurrentRun == null) {
                gameState.StartNewRun();
            }

            return gameState;
        }

        private static GameRunSaveData SaveGameRun(GameRunState gameRunState) {
            if (gameRunState == null) {
                return null;
            }
            
            var saveData = new GameRunSaveData();

            saveData.deck = SaveDeck(gameRunState.Deck);

            return saveData;
        }

        private static GameRunState LoadGameRun(GameRunSaveData saveData) {
            if (saveData == null || saveData.isNull) {
                return null;
            }

            var gameRunState = new GameRunState();

            gameRunState.Deck = LoadDeck(saveData.deck);

            return gameRunState;
        }

        private static CardStats[] SaveDeck(Deck deck) {
            if (deck == null || deck.Size == 0) {
                return null;
            }
            
            return deck.Cards.Select(card => card.UpcastedStats).ToArray();
        }

        private static Deck LoadDeck(IReadOnlyCollection<CardStats> saveData) {
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