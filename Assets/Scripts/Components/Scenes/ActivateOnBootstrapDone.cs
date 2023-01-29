using System;
using UnityEngine;

namespace Components.Scenes {
    public class ActivateOnBootstrapDone : MonoBehaviour {
        
        private void Awake() {
            if (!Game.IsBootstrapDone) {
                gameObject.SetActive(false);

                Game.BootstrapDone += OnBootstrapDone;
            }
        }

        private void OnDestroy() {
            Game.BootstrapDone -= OnBootstrapDone;
        }

        private void OnBootstrapDone() {
            gameObject.SetActive(true);
        }
    }
}