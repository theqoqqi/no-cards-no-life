using UnityEngine;

namespace Core.Choices {
    public abstract class ChoiceMetadata : ScriptableObject {
        
        [SerializeField] private Sprite sprite;

        public Sprite Sprite => sprite;
    }
}