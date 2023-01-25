using System.Collections.Generic;
using System.Linq;
using Components.Boards;
using UnityEngine;

namespace Core.Cards {
    public class AttackCard : Card {

        private readonly int maxDistance;

        private readonly int damage;

        protected override int TopLeftValue => maxDistance;

        protected override int TopRightValue => damage;

        public AttackCard(int maxDistance, int damage) : base("Attack") {
            this.maxDistance = maxDistance;
            this.damage = damage;
        }

        public override IEnumerable<Vector2Int> GetSelectableCells(Board board) {
            return board.GetEnemies()
                    .Select(enemy => (Vector2Int) enemy.CellPosition)
                    .Where(cellPosition => IsInRange(board.Player, cellPosition, maxDistance));
        }
    }
}