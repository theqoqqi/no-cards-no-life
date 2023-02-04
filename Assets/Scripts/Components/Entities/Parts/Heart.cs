using Components.Utils;
using UnityEngine;

namespace Components.Entities.Parts {
    public class Heart : MonoBehaviour {

        [SerializeField] private EasingTransform easingTransform;

        public EasingTransform EasingTransform => easingTransform;

        protected int Index { get; private set; }

        protected virtual void Awake() {
            EasingTransform.EasingSpeed = 20f;
        }

        public void SetIndex(int index) {
            Index = index;
        }
    }
}