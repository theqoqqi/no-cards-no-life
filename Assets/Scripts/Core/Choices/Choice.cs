using System.Threading.Tasks;
using Components.Locations;
using UnityEngine;

namespace Core.Choices {
    public abstract class Choice {

        protected abstract ChoiceMetadata UpcastedMetadata { get; }

        public Sprite Sprite => UpcastedMetadata.Sprite;

        public abstract bool CanBeApplied(Location location);

        public abstract Task Apply(Location location);
    }
}