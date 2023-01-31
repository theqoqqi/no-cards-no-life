
using System.Collections.Generic;
using System.Threading.Tasks;
using Components.Boards;
using Components.Entities;
using Core.Cards.Stats;
using UnityEngine;

namespace Core.Cards {
    public abstract partial class Card {
        
        private readonly CardMetadata metadata;
        
        public abstract CardStats UpcastedStats { get; }

        public Sprite ActionSprite => metadata.ActionSprite;

        protected virtual int TopLeftValue { get; } = 0;

        protected virtual int TopRightValue { get; } = 0;

        public Corner? TopLeft => GetCorner(metadata.TopLeft, TopLeftValue);

        public Corner? TopRight => GetCorner(metadata.TopRight, TopRightValue);

        protected Card(string typeName) {
            metadata = Resources.Load<CardMetadata>("ScriptableObjects/CardTypes/" + typeName);
        }

        public abstract IEnumerable<Vector2Int> GetSelectableCells(Board board);

        public abstract Task Use(Board board, Vector3Int cellPosition);

        private static Corner? GetCorner(CardMetadata.CornerMetadata metadata, int value) {
            if (metadata == null || value == 0) {
                return null;
            }

            return new Corner(value, metadata.BackgroundSprite, metadata.TypeSprite);
        }

        protected static bool IsInRange(Player player, Vector2Int to, int distance) {
            return IsInRange(player.CellPosition, to, distance);
        }

        private static bool IsInRange(Vector3Int from, Vector2Int to, int distance) {
            return Board.GetDistanceBetween(from, (Vector3Int) to) < distance + 1;
        }
    }
}