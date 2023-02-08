using Core.Sectors;
using Qoqqi.Inspector.Runtime;
using UnityEngine;

namespace Components.Locations {
    public class LocationSector : LocationCell {

        [SerializeReference, SubclassPicker(true)]
        private Sector sector;

        public Sector Sector => sector;
    }
}