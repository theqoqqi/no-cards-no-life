using UnityEngine;

namespace Components.Combats {
    public class SkipMovePad : MonoBehaviour {

        [SerializeField] private Vector2 heartPosition;
        
        public Vector3 HeartPosition => transform.position + (Vector3) heartPosition;

        public bool IsMouseOver { get; private set; }

        private void OnMouseOver() {
            IsMouseOver = true;
        }

        private void OnMouseExit() {
            IsMouseOver = false;
        }
    }
}