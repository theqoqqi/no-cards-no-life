using System.Threading.Tasks;
using Components.Locations;
using UnityEngine;

namespace Core.Choices {
    public abstract class Choice : ScriptableObject {
        
        [SerializeField] private Sprite sprite;

        public Sprite Sprite => sprite;

        public abstract bool CanBeApplied(Location location);

        public abstract Task Apply(Location location);
    }
}