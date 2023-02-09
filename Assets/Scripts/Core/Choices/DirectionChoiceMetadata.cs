using Core.Util;
using UnityEngine;

namespace Core.Choices {
    [CreateAssetMenu(fileName = "Direction Choice", menuName = "NCNL/Choices/Direction Choice", order = 0)]
    public class DirectionChoiceMetadata : ChoiceMetadata {

        [SerializeField] private Direction direction;

        public Direction Direction => direction;
    }
}