using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Boards;
using Components.Entities;
using Core.Cards.Stats;
using UnityEngine;

namespace Core.Cards {
    public class AttackCard : GenericCard<AttackCardStats> {

        protected override int TopLeftValue => Stats.maxDistance;

        protected override int TopRightValue => Stats.damage;

        public AttackCard(int maxDistance, int damage)
                : base("Attack", new AttackCardStats(maxDistance, damage)) {
        }

        public override IEnumerable<Vector2Int> GetSelectableCells(Board board) {
            return board.GetEnemies()
                    .Select(enemy => (Vector2Int) enemy.CellPosition)
                    .Where(cellPosition => IsInRange(board.Player, cellPosition, Stats.maxDistance));
        }

        public override async Task Use(Board board, Vector3Int cellPosition) {
            var enemy = board.GetObjectAtCell<Enemy>((Vector3Int) (Vector2Int) cellPosition);
            var player = board.Player;

            await player.StartAttack(enemy, Stats.damage);
        }
    }
}