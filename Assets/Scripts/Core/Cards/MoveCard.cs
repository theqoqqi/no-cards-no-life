using System.Collections.Generic;
using Components.Boards;
using Core.Pathfinding;
using UnityEngine;

namespace Core.Cards {
    public class MoveCard : Card {

        private readonly int maxDistance;

        protected override int TopLeftValue => maxDistance;

        public MoveCard(int maxDistance) : base("Move") {
            this.maxDistance = maxDistance;
        }

        public override IEnumerable<Vector2Int> GetSelectableCells(Board board) {
            var search = new AStarSearch(board.PassabilityGrid, board.Player.CellPosition);
            
            return search.FindPositions(new AStarSearch.FindOptions((_, distance) => {
                return distance <= maxDistance;
            }));
        }

        public override void Use(Board board, Vector3Int cellPosition) {
            
        }
    }
}