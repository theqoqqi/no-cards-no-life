using System.Collections.Generic;
using Components.Boards;
using UnityEngine;

namespace Core.Cards {
    public class AttackCard : Card {

        private readonly int maxDistance;

        private readonly int damage;

        public override int TopLeftValue => maxDistance;

        public override int TopRightValue => damage;

        public AttackCard(int maxDistance, int damage) : base("Attack") {
            this.maxDistance = maxDistance;
            this.damage = damage;
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