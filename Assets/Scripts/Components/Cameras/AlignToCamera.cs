using UnityEngine;

namespace Components.Cameras {
    public class AlignToCamera : MonoBehaviour {

        [SerializeField] private AbstractCamera toCamera;

        [SerializeField] private Vector2 screenPosition;

        [SerializeField] private Vector3 offset;

        private void Update() {
            var worldPosition = toCamera.Camera.ViewportToWorldPoint(screenPosition);
            worldPosition.z = transform.position.z;

            transform.position = worldPosition + offset;
        }
    }
}