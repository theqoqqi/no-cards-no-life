using System;
using Core.Cards.Stats;
using Core.Util;
using Qoqqi.Inspector.Runtime;
using UnityEngine;

namespace Core.Saves {
    [Serializable]
    public class GameRunSaveData : NullableObject {
        
        [SerializeReference, SubclassPicker(true)]
        public CardStats[] deck;
    }
}