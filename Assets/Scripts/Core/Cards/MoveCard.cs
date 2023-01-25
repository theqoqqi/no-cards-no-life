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
            var search = new AStarSearch(board.PassabilityGrid, board.Player.CellPosition);

            return search.FindPositions(findOptions);
        }

        public override async Task Use(Board board, Vector3Int cellPosition) {
            var positions = FindPath(board, cellPosition);

            foreach (var position in positions) {
                await board.Player.Body.StartMoveTo(position);
            }
        }

        private IEnumerable<Vector3> FindPath(Board board, Vector3Int cellPosition) {
            var search = new AStarSearch(board.PassabilityGrid, board.Player.CellPosition, new[] {cellPosition});
            
            return search.FindPath(findOptions)
                    .Skip(1)
                    .Select(position => ((Vector3Int) position).CellToWorld());
        }
    }
}