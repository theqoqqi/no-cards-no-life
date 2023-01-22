using Core.Events;
using UnityEngine;

namespace Components {
    public class Game : MonoBehaviour {

        private void Awake() {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            
            GameEvents.Instance.On<CardUsedEvent>(Debug.Log);
        }
    }
}