using Components.Boards;
using Core.Events;
using Core.Events.Levels;
using UnityEngine;

namespace Components.Cameras {
    public class MainCamera : MonoBehaviour {

        public Camera Camera { get; private set; }

        [SerializeField] private float paddingUnits = 4; 
        
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
        }

        private void Update() {
            Setup();
        }

        private void OnEnable() {
            GameEvents.Instance.On<LevelLoadedEvent>(OnLevelLoaded);
        }

        private void OnDisable() {
            GameEvents.Instance.Off<LevelLoadedEvent>(OnLevelLoaded);
        }

        private void OnLevelLoaded(LevelLoadedEvent e) {
            board = FindObjectOfType<Board>();
            
            Setup();
        }

        private void Setup() {
            if (!board || !board.Player) {
                return;
            }

            var center = board.CenterPosition;
            var minX = Mathf.Min(board.Bounds.xMin + paddingUnits, center.x);
            var minY = Mathf.Min(board.Bounds.yMin + paddingUnits, center.y);
            var maxX = Mathf.Max(board.Bounds.xMax - paddingUnits, center.x);
            var maxY = Mathf.Max(board.Bounds.yMax - paddingUnits, center.y);

            var position = board.Player.transform.position;

            position.x = Mathf.Clamp(position.x, minX, maxX);
            position.y = Mathf.Clamp(position.y, minY, maxY);
            
            Position = position + GetCameraOffset();
        }

        private Vector3 GetCameraOffset() {
            return Vector3.down * 1.5f;
        }
    }
}