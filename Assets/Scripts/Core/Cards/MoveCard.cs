namespace Core.Cards {
    public class MoveCard : Card {

        private readonly int maxDistance;

        public override int TopLeftValue => maxDistance;

        public MoveCard(int maxDistance) : base("Move") {
            this.maxDistance = maxDistance;
        }
    }
}