using System.Collections.Generic;
using System.Threading.Tasks;
using Components.Boards;
using Core.Cards.Stats;
using Core.Pathfinding;
using UnityEngine;

namespace Core.Cards {
    public class MoveCard : GenericCard<MoveCardStats> {

        protected override int TopLeftValue => Stats.maxDistance;

        private readonly AStarSearch.FindOptions findOptions;

        public MoveCard(int maxDistance) : this(new MoveCardStats(maxDistance)) {
            findOptions = new AStarSearch.FindOptions((_, distance) => {
                return distance <= maxDistance;
            });
        }

        private MoveCard(MoveCardStats stats) : base("Move", stats) {
        }

        public override IEnumerable<Vector2Int> GetSelectableCells(Board board) {
            return board.Pathfinder.FindPositions(board.Player, findOptions);
        }

        public override async Task Use(Board board, Vector3Int cellPosition) {
            var positions = board.Pathfinder.FindPath(board.Player, cellPosition, findOptions);

            foreach (var position in positions) {
                await board.Player.Body.StartMoveTo(position);
            }
        }
    }
}