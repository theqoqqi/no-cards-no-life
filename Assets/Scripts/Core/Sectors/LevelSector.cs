using System;
using System.Threading.Tasks;
using Components;
using Components.Locations;
using Udar.SceneManager;
using UnityEngine;

namespace Core.Sectors {
    [Serializable]
    public class LevelSector : Sector {

        [SerializeField] private SceneField level;
        
        public string LevelName => level.Name;

        public override async Task Visit(Location location) {
            await Game.Instance.SceneManager.LoadLevel(LevelName);
        }
    }
}