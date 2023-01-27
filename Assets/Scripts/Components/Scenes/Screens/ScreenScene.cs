using UnityEngine;

namespace Components.Scenes.Screens {
    public class ScreenScene : MonoBehaviour {

        public void Unload() {
            Destroy(gameObject);
        }
    }
}