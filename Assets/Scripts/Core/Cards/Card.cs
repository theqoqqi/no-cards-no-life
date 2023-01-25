
using System.Collections.Generic;
using Components.Boards;
using Components.Entities;
using Core.Util;
using UnityEngine;

namespace Core.Cards {
    public abstract class Card {
        
        private readonly CardMetadata metadata;

        public Sprite ActionSprite => metadata.ActionSprite;

        protected virtual int TopLeftValue { get; } = 0;

        protected virtual int TopRightValue { get; } = 0;

        public Corner? TopLeft => GetCorner(metadata.TopLeft, TopLeftValue);

        public Corner? TopRight => GetCorner(metadata.TopRight, TopRightValue);

        protected Card(string typeName) {
            metadata = Resources.Load<CardMetadata>("ScriptableObjects/CardTypes/" + typeName);
        }

        private Corner? GetCorner(CardMetadata.CornerMetadata metadata, int value) {
            if (metadata == null || value == 0) {
                return null;
            }

            return new Corner(TopLeftValue, metadata.BackgroundSprite, metadata.TypeSprite);
        }

        public abstract IEnumerable<Vector2Int> GetSelectableCells(Board board);

        public abstract void Use(Board board, Vector3Int cellPosition);

        public readonly struct Corner {

            public int Value { get; }

            public Sprite BackgroundSprite { get; }

            public Sprite TypeSprite { get; }

            public Corner(int value, Sprite backgroundSprite, Sprite typeSprite) {
                Value = value;
                BackgroundSprite = backgroundSprite;
                TypeSprite = typeSprite;
            }
        }

        protected static bool IsInRange(Player player, Vector2Int to, int distance) {
            return IsInRange(player.CellPosition, to, distance);
        }

        protected static bool IsInRange(Vector3Int from, Vector2Int to, int distance) {
            return ((Vector2Int) from).ManhattanDistance(to) < distance + 1;
        }
    }
}