using System;
using UnityEngine;

namespace Components.Cards {
    public class CardDigit : MonoBehaviour {

        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private Sprite[] digits;

        public void SetDigit(int digit) {
            if (digit < 0 || digit > 9) {
                throw new ArgumentException(nameof(digit) + " must be in range 0-9");
            }

            spriteRenderer.sprite = digits[digit];
        }
    }
}