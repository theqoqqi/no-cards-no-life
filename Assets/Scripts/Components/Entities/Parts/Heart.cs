using Components.Utils;
using UnityEngine;

namespace Components.Entities.Parts {
    public class Heart : MonoBehaviour {

        [SerializeField] private EasingTransform easingTransform;

        public EasingTransform EasingTransform => easingTransform;

        private void Awake() {
            EasingTransform.EasingSpeed = 20f;
        }
    }
}