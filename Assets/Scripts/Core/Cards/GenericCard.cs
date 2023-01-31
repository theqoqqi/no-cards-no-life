using Core.Cards.Stats;

namespace Core.Cards {
    public abstract class GenericCard<T> : Card where T : CardStats {
        
        public override CardStats UpcastedStats => Stats;

        protected readonly T Stats;

        protected GenericCard(string typeName, T stats) : base(typeName) {
            Stats = stats;
        }
    }
}