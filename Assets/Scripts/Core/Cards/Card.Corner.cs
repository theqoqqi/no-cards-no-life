using UnityEngine;

namespace Core.Cards {
    public abstract partial class Card {
        public readonly struct Corner {

            public int Value { get; }

            public Sprite BackgroundSprite { get; }

            public Sprite TypeSprite { get; }

            public Corner(int value, Sprite backgroundSprite, Sprite typeSprite) {
                Value = value;
                BackgroundSprite = backgroundSprite;
                TypeSprite = typeSprite;
            }
        }
    }
}