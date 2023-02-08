using Core.Choices;
using Core.Util;
using UnityEngine;

namespace Components.Locations.Choices {
    [CreateAssetMenu(fileName = "Direction Choice", menuName = "NCNL/Choices/Direction Choice", order = 0)]
    public class DirectionChoice : Choice {

        [SerializeField] private Direction direction;

        public Direction Direction => direction;
    }
}