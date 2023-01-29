using System.Collections.Generic;
using System.Linq;
using Components.Cameras;
using Components.Entities;
using Components.Combats;
using Core.Events;
using Core.Events.Cards;
using Core.Pathfinding;
using Core.Util;
using UnityEngine;

namespace Components.Boards {
    public class Board : MonoBehaviour {

        private TerrainTilemap terrainTilemap;

        private GridTilemap gridTilemap;

        private EntityGrid entityGrid;

        private MainCamera mainCamera;

        private BoardPassabilityGrid passabilityGrid;

        public BoardPassabilityGrid PassabilityGrid => passabilityGrid;

        private Player player;

        public Player Player => player;
        
        public IEnumerable<TurnPerformer> TurnPerformers => entityGrid.GetObjectsOfType<TurnPerformer>();

        public Vector3 CenterPosition => terrainTilemap.CenterPosition;

        public int Height => terrainTilemap.Height;

        public BoundsInt Bounds => terrainTilemap.CellBounds;

        private void Awake() {
            terrainTilemap = GetComponentInChildren<TerrainTilemap>();
            gridTilemap = GetComponentInChildren<GridTilemap>();
            entityGrid = GetComponentInChildren<EntityGrid>();
            mainCamera = FindObjectOfType<MainCamera>();
            player = FindObjectOfType<Player>();

            passabilityGrid = new BoardPassabilityGrid(this);
        }

        private void Update() {
            var mouseCellPosition = mainCamera.Camera.ScreenToWorldPoint(Input.mousePosition).WorldToCell();

            if (IsOnBoard(mouseCellPosition)) {
                gridTilemap.SetHoveredCell((Vector2Int) mouseCellPosition);
            }
            else {
                gridTilemap.ClearHoveredCell();
            }
        }

        private void OnEnable() {
            GameEvents.Instance.On<CardReleasedOnBoardEvent>(OnCardReleasedOnBoard);
            GameEvents.Instance.On<CardDraggedToBoardEvent>(OnCardDraggedToBoard);
            GameEvents.Instance.On<CardDraggedFromBoardEvent>(OnCardDraggedFromBoard);
        }

        private void OnDisable() {
            GameEvents.Instance.Off<CardReleasedOnBoardEvent>(OnCardReleasedOnBoard);
            GameEvents.Instance.Off<CardDraggedToBoardEvent>(OnCardDraggedToBoard);
            GameEvents.Instance.Off<CardDraggedFromBoardEvent>(OnCardDraggedFromBoard);
        }

        private void OnCardDraggedToBoard(CardDraggedToBoardEvent e) {
            var selectableCells = e.Card.GetSelectableCells(this);
            
            gridTilemap.SetSelectableCells(selectableCells);
        }

        private void OnCardDraggedFromBoard(CardDraggedFromBoardEvent e) {
            gridTilemap.ClearSelectableCells();
        }

        private void OnCardReleasedOnBoard(CardReleasedOnBoardEvent e) {
            var mouseCellPosition = mainCamera.Camera.ScreenToWorldPoint(Input.mousePosition).WorldToCell();

            if (gridTilemap.IsSelectable(mouseCellPosition)) {
                GameEvents.Instance.Enqueue<CardReleasedOnSelectableCellEvent>().With(e.Card, mouseCellPosition);
            }
            
            gridTilemap.ClearSelectableCells();
        }

        public bool IsOnBoard(Vector3Int position) {
            return terrainTilemap.IsOnBoard(position);
        }

        public bool IsCellPassable(Vector3Int cellPosition) {
            return terrainTilemap.IsCellPassable(cellPosition)
                   && entityGrid.IsCellPassable(cellPosition);
        }

        public bool HasBodyAtCell(Vector3Int cellPosition) {
            return entityGrid.GetBodyAtCell(cellPosition);
        }

        public T GetObjectAtCell<T>(Vector3Int cellPosition) where T : EntityBehaviour {
            return entityGrid.GetObjectAtCell<T>(cellPosition);
        }

        public IEnumerable<Enemy> GetEnemies() {
            return entityGrid.GetObjectsOfType<Enemy>();
        }

        public bool HasEnemies() {
            return GetEnemies().Any();
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
            var search = new AStarSearch(PassabilityGrid, from, to);
            
            return search.FindPath(findOptions)
                    .Skip(1)
                    .Select(position => ((Vector3Int) position).CellToWorld());
        }

        public IEnumerable<Vector2Int> FindPositions(BaseEntity originEntity, AStarSearch.FindOptions findOptions) {
            return FindPositions(originEntity.CellPosition, findOptions);
        }

        public IEnumerable<Vector2Int> FindPositions(Vector3Int origin, AStarSearch.FindOptions findOptions) {
            var search = new AStarSearch(PassabilityGrid, origin);

            return search.FindPositions(findOptions);
        }

        public static int GetDistanceBetween(Enemy enemy, Player boardPlayer) {
            return GetDistanceBetween(enemy.CellPosition, boardPlayer.CellPosition);
        }

        public static int GetDistanceBetween(Vector3Int a, Vector3Int b) {
            var dx = Mathf.Abs(a.x - b.x);
            var dy = Mathf.Abs(a.y - b.y);

            return Mathf.Max(dx, dy);
        }
    }
}