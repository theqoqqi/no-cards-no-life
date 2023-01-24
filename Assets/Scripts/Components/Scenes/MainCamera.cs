using Components.Boards;
using UnityEngine;

namespace Components.Scenes {
    public class MainCamera : MonoBehaviour {

        public Camera Camera { get; private set; }

        private Board board;

        private Vector3 Position {
            get => transform.position;
            set {
                var x = value.x;
                var y = value.y;
                var z = transform.position.z;
            
                transform.position = new Vector3(x, y, z);
            }
        }

        private void Start() {
            board = FindObjectOfType<Board>();
            Camera = GetComponent<Camera>();

            Position = board.CenterPosition;
            SetSize(board.Height);
            Adjust(board.Height);
        }

        private void SetSize(int boardHeight) {
            Camera.orthographicSize = boardHeight / 2f * 1.6f;
        }

        private void Adjust(int boardHeight) {
            transform.position += Vector3.down * boardHeight * 0.25f;
        }
    }
}