using Components.Entities;
using Core.Util;
using UnityEngine;

namespace Components.Locations {
    public class LocationPlayer : MonoBehaviour {
        
        public Vector3Int CellPosition => transform.position.WorldToCell();
        
        [SerializeField] private GridAlignedBody body;

        public GridAlignedBody Body => body;
    }
}