using System;
using Core.Cards.Stats;
using Core.GameStates;
using Core.Util;
using Qoqqi.Inspector.Runtime;
using UnityEngine;

namespace Core.Saves {
    [Serializable]
    public class GameSaveData : NullableObject {

        [SerializeReference, SubclassPicker(true)]
        public CardStats[] firstRunDeck;

        [SerializeReference, SubclassPicker(true)]
        public CardStats[] starterDeck;

        [SerializeField, NullableField]
        public GameRunSaveData currentRun;
        
        internal static GameSaveData Save(GameState gameState) {
            return new GameSaveData {
                    firstRunDeck = SaveDataUtils.SaveDeck(gameState.FirstRunDeck),
                    starterDeck = SaveDataUtils.SaveDeck(gameState.StarterDeck),
                    currentRun = GameRunSaveData.Save(gameState.CurrentRun)
            };
        }

        internal static GameState Load(GameSaveData saveData) {
            return new GameState {
                    FirstRunDeck = SaveDataUtils.LoadDeck(saveData.firstRunDeck),
                    StarterDeck = SaveDataUtils.LoadDeck(saveData.starterDeck),
                    CurrentRun = GameRunSaveData.Load(saveData.currentRun)
            };
        }
    }
}