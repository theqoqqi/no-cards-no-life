using UnityEngine;

namespace Components.Cameras {
    public abstract class AbstractCamera : MonoBehaviour {
        
        public Camera Camera { get; protected set; }

        protected Vector3 Position {
            get => transform.position;
            set {
                var x = value.x;
                var y = value.y;
                var z = transform.position.z;
            
                transform.position = new Vector3(x, y, z);
            }
        }

        private void Awake() {
            Camera = GetComponent<Camera>();
        }
    }
}