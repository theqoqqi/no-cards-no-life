using UnityEngine;

namespace Core.Choices {
    public class Choice : ScriptableObject {
        
        [SerializeField] private Sprite sprite;

        public Sprite Sprite => sprite;
    }
}