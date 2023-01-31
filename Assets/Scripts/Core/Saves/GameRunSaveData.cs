using System;
using Core.Cards.Stats;
using Core.GameStates;
using Core.Util;
using Qoqqi.Inspector.Runtime;
using UnityEngine;

namespace Core.Saves {
    [Serializable]
    public class GameRunSaveData : NullableObject {

        [SerializeReference, SubclassPicker(true)]
        public CardStats[] deck;

        internal static GameRunSaveData Save(GameRunState gameRunState) {
            if (gameRunState == null) {
                return null;
            }

            return new GameRunSaveData {
                    deck = SaveDataUtils.SaveDeck(gameRunState.Deck)
            };
        }

        internal static GameRunState Load(GameRunSaveData saveData) {
            if (saveData == null || saveData.isNull) {
                return null;
            }

            return new GameRunState {
                    Deck = SaveDataUtils.LoadDeck(saveData.deck)
            };
        }
    }
}