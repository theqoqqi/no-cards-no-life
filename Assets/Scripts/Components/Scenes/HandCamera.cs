using UnityEngine;

namespace Components.Scenes {
    public class HandCamera : MonoBehaviour {

        [SerializeField] public new Camera camera;

        public Camera Camera => camera;
    }
}