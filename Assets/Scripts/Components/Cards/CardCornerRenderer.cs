using Core.Cards;
using TMPro;
using UnityEngine;

namespace Components.Cards {
    public class CardCornerRenderer : MonoBehaviour {

        [SerializeField] private SpriteRenderer backgroundSpriteRenderer;

        [SerializeField] private SpriteRenderer typeSpriteRenderer;

        [SerializeField] private TextMeshPro textMesh;

        public void Setup(Card.Corner? corner) {
            gameObject.SetActive(corner.HasValue);
            
            if (!corner.HasValue) {
                return;
            }

            Setup(corner.Value);
        }

        private void Setup(Card.Corner corner) {
            backgroundSpriteRenderer.sprite = corner.BackgroundSprite;
            typeSpriteRenderer.sprite = corner.TypeSprite;
            textMesh.text = corner.Value.ToString();
        }

        public void SetSortingOrder(int order) {
            textMesh.sortingOrder = order;
        }

        public void SetOpacity(int opacity) {
            var color = textMesh.color;
            
            color.a = opacity;

            textMesh.color = color;
        }
    }
}