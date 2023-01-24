using Core.Events;
using UnityEngine;

namespace Components.Boards {
    public class Board : MonoBehaviour {

        private TerrainTilemap terrainTilemap;

        private EntityGrid entityGrid;

        public Vector3 CenterPosition => terrainTilemap.CenterPosition;

        public int Height => terrainTilemap.Height;

        private void Awake() {
            terrainTilemap = GetComponentInChildren<TerrainTilemap>();
            entityGrid = GetComponentInChildren<EntityGrid>();
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

        private void OnCardDraggedFromBoard(CardDraggedFromBoardEvent e) {
            Debug.Log("CardDraggedFromBoardEvent");
        }

        private void OnCardDraggedToBoard(CardDraggedToBoardEvent e) {
            Debug.Log("CardDraggedToBoardEvent");
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