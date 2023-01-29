using System.Collections.Generic;
using System.Linq;
using Components.Entities;
using Core.Pathfinding;
using Core.Util;
using UnityEngine;

namespace Components.Boards {
    public class BoardPathfinder {

        private readonly BoardPassabilityGrid passabilityGrid;

        public BoardPathfinder(Board board) {
            passabilityGrid = new BoardPassabilityGrid(board);
        }

        public IEnumerable<Vector3> FindPath(BaseEntity fromEntity, BaseEntity toEntity, AStarSearch.FindOptions findOptions) {
            return FindPath(fromEntity.CellPosition, toEntity.CellPosition, findOptions);
        }

        public IEnumerable<Vector3> FindPath(BaseEntity fromEntity, Vector3Int to, AStarSearch.FindOptions findOptions) {
            return FindPath(fromEntity.CellPosition, to, findOptions);
        }

        public IEnumerable<Vector3> FindPath(Vector3Int from, Vector3Int to, AStarSearch.FindOptions findOptions) {
            return FindPath(from, new[] {to}, findOptions);
        }

        public IEnumerable<Vector3> FindPath(Vector3Int from, IEnumerable<Vector3Int> to, AStarSearch.FindOptions findOptions) {
            var search = new AStarSearch(passabilityGrid, from, to);
            
            return search.FindPath(findOptions)
                    .Skip(1)
                    .Select(position => ((Vector3Int) position).CellToWorld());
        }

        public IEnumerable<Vector2Int> FindPositions(BaseEntity originEntity, AStarSearch.FindOptions findOptions) {
            return FindPositions(originEntity.CellPosition, findOptions);
        }

        public IEnumerable<Vector2Int> FindPositions(Vector3Int origin, AStarSearch.FindOptions findOptions) {
            var search = new AStarSearch(passabilityGrid, origin);

            return search.FindPositions(findOptions);
        }
    }
}