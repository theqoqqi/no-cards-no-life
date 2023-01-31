using System;

namespace Core.Cards.Stats {
    [Serializable]
    public class MoveCardStats : CardStats {

        public int maxDistance;

        public MoveCardStats() {
            
        }

        public MoveCardStats(int maxDistance) {
            this.maxDistance = maxDistance;
        }
    }
}