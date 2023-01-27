using UnityEngine;

namespace Components.Scenes {
    public class ScreenScene : MonoBehaviour {

        public void Unload() {
            Destroy(gameObject);
        }
    }
}