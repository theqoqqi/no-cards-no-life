using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Boards;
using Components.Entities;
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

        public override async Task Use(Board board, Vector3Int cellPosition) {
            var enemy = board.GetObjectAtCell<Enemy>((Vector3Int) (Vector2Int) cellPosition);
            var player = board.Player;

            await player.StartAttack(enemy, damage);
        }
    }
}