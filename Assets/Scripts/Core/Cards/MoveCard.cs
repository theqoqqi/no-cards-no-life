using System.Collections.Generic;
using Components.Boards;
using UnityEngine;

namespace Core.Cards {
    public class MoveCard : Card {

        private readonly int maxDistance;

        public override int TopLeftValue => maxDistance;

        public MoveCard(int maxDistance) : base("Move") {
            this.maxDistance = maxDistance;
        }

        public override IEnumerable<Vector2Int> GetSelectableCells(Board board) {
            var cells = new List<Vector2Int>();
            
            foreach (var cellPosition in board.Bounds.allPositionsWithin) {
                cells.Add((Vector2Int) cellPosition);
            }

            return cells;
        }
    }
}