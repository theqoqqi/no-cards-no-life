using System;
using Components.Scenes;
using Core.Events;
using Core.Util;
using UnityEngine;

namespace Components.Boards {
    public class Board : MonoBehaviour {

        private TerrainTilemap terrainTilemap;

        private GridTilemap gridTilemap;

        private EntityGrid entityGrid;

        private MainCamera mainCamera;

        public Vector3 CenterPosition => terrainTilemap.CenterPosition;

        public int Height => terrainTilemap.Height;

        public BoundsInt Bounds => terrainTilemap.CellBounds;

        private void Awake() {
            terrainTilemap = GetComponentInChildren<TerrainTilemap>();
            gridTilemap = GetComponentInChildren<GridTilemap>();
            entityGrid = GetComponentInChildren<EntityGrid>();
            mainCamera = FindObjectOfType<MainCamera>();
        }

        private void Update() {
            var mouseCellPosition = mainCamera.Camera.ScreenToWorldPoint(Input.mousePosition).WorldToCell();

            if (IsOnBoard(mouseCellPosition)) {
                gridTilemap.SetSelectedCell((Vector2Int) mouseCellPosition);
            }
            else {
                gridTilemap.ClearSelectedCell();
            }
        }

        private void OnEnable() {
            GameEvents.Instance.On<CardUsedEvent>(OnCardUsed);
            GameEvents.Instance.On<CardDraggedToBoardEvent>(OnCardDraggedToBoard);
            GameEvents.Instance.On<CardDraggedFromBoardEvent>(OnCardDraggedFromBoard);
        }

        private void OnDisable() {
            GameEvents.Instance.Off<CardUsedEvent>(OnCardUsed);
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

        private void OnCardUsed(CardUsedEvent e) {
            Game.Instance.GameState.CurrentHand.RemoveCard(e.Card);
        }

        public bool IsOnBoard(Vector3Int position) {
            return terrainTilemap.IsOnBoard(position);
        }

        public bool IsCellPassable(Vector3Int cellPosition) {
            return terrainTilemap.IsCellPassable(cellPosition)
                   && entityGrid.IsCellPassable(cellPosition);
        }
    }
}