using Core.Events;
using Core.Events.Levels;
using UnityEngine;

namespace Components.Levels {
    public class Level : MonoBehaviour {

        private void Start() {
            GameEvents.Instance.Dispatch<LevelLoadedEvent>(e => {
                e.Setup(this);
            });
        }
    }
}