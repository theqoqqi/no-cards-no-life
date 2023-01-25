using UnityEngine;

namespace Components.Scenes {
    public class AlignToCamera : MonoBehaviour {

        [SerializeField] private HandCamera handCamera;

        [SerializeField] private Vector2 screenPosition;

        [SerializeField] private Vector3 offset;

        private void Update() {
            var worldPosition = handCamera.Camera.ViewportToWorldPoint(screenPosition);
            worldPosition.z = transform.position.z;

            transform.position = worldPosition + offset;
        }
    }
}