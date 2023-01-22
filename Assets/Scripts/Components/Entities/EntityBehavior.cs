using Core.Util;
using UnityEngine;

namespace Components.Entities {
    public class EntityBehaviour : MonoBehaviour {
        
        public Vector3Int CellPosition => transform.position.WorldToCell();
        
        // protected World World;
        //
        // protected Level Level;

        protected virtual void Awake() {
            // World = FindObjectOfType<World>();
            // Level = FindObjectOfType<Level>();
        }
    }
}