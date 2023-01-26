using UnityEngine;

namespace Components.Cameras {
    public class HandCamera : MonoBehaviour {

        [SerializeField] public new Camera camera;

        public Camera Camera => camera;
    }
}