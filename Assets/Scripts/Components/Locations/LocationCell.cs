using Core.Util;
using UnityEngine;

namespace Components.Locations {
    public class LocationCell : MonoBehaviour {

        public Vector3Int CellPosition => transform.position.WorldToCell();
    }
}