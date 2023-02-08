using UnityEngine;

namespace Components.Utils {
    public class EasingTransform : MonoBehaviour {

        [SerializeField] private float easingSpeed = 1;

        public Vector3 TargetLocalPosition { get; set; }

        public Quaternion TargetLocalRotation { get; set; }

        public Vector3 TargetLocalScale { get; set; }

        public float EasingSpeed {
            get => easingSpeed;
            set => easingSpeed = value;
        }

        private void Awake() {
            TargetLocalPosition = transform.localPosition;
            TargetLocalRotation = transform.localRotation;
            TargetLocalScale = transform.localScale;
        }

        private void Update() {
            UpdateTransform();
        }

        private void UpdateTransform() {
            transform.localPosition = Vector3.Lerp(
                    transform.localPosition,
                    TargetLocalPosition,
                    Time.deltaTime * EasingSpeed
            );

            transform.localRotation = Quaternion.Lerp(
                    transform.localRotation,
                    TargetLocalRotation,
                    Time.deltaTime * EasingSpeed
            );

            transform.localScale = Vector3.Lerp(
                    transform.localScale,
                    TargetLocalScale,
                    Time.deltaTime * EasingSpeed
            );
        }

        public void SetTargetState(Vector3 position, Quaternion rotation, Vector3 scale) {
            TargetLocalPosition = position;
            TargetLocalRotation = rotation;
            TargetLocalScale = scale;
        }
    }
}