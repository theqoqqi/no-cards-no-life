using Components.Boards;
using UnityEngine;

namespace Components.Scenes {
    public class MainCamera : MonoBehaviour {

        private new Camera camera;

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
            camera = GetComponent<Camera>();

            Position = board.CenterPosition;
            SetSize(board.Height);
            Adjust(board.Height);
        }

        private void SetSize(int boardHeight) {
            camera.orthographicSize = boardHeight / 2f * 1.6f;
        }

        private void Adjust(int boardHeight) {
            transform.position += Vector3.down * boardHeight * 0.25f;
        }
    }
}