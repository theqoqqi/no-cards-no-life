using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Boards;
using Core.Pathfinding;
using Core.Util;
using UnityEngine;

namespace Core.Cards {
    public class MoveCard : Card {

        private readonly int maxDistance;

        protected override int TopLeftValue => maxDistance;

        private readonly AStarSearch.FindOptions findOptions;

        public MoveCard(int maxDistance) : base("Move") {
            this.maxDistance = maxDistance;
            
            findOptions = new AStarSearch.FindOptions((_, distance) => {
                return distance <= maxDistance;
            });
        }

        public override IEnumerable<Vector2Int> GetSelectableCells(Board board) {
            return board.FindPositions(board.Player, findOptions);
        }

        public override async Task Use(Board board, Vector3Int cellPosition) {
            var positions = board.FindPath(board.Player, cellPosition, findOptions);

            foreach (var position in positions) {
                await board.Player.Body.StartMoveTo(position);
            }
        }
    }
}