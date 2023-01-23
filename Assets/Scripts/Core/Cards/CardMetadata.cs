using System;
using UnityEngine;

namespace Core.Cards {
    [CreateAssetMenu(fileName = "Action", menuName = "NCNL/Cards/Card Type", order = 0)]
    public class CardMetadata : ScriptableObject {
        
        public Sprite ActionSprite;

        public CornerMetadata TopLeft;

        public CornerMetadata TopRight;
        
        [Serializable]
        public class CornerMetadata {

            public Sprite BackgroundSprite;

            public Sprite TypeSprite;
        }
    }
}