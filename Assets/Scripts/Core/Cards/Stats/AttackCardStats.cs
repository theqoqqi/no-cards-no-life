using System;

namespace Core.Cards.Stats {
    [Serializable]
    public class AttackCardStats : CardStats {

        public int maxDistance;

        public int damage;

        public AttackCardStats() {
            
        }

        public AttackCardStats(int maxDistance, int damage) {
            this.maxDistance = maxDistance;
            this.damage = damage;
        }
    }
}