using System;
using Core.Cards.Stats;
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
    }
}