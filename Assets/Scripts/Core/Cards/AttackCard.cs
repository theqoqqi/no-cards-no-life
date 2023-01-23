namespace Core.Cards {
    public class AttackCard : Card {

        private readonly int maxDistance;

        private readonly int damage;

        public override int TopLeftValue => maxDistance;

        public override int TopRightValue => damage;

        public AttackCard(int maxDistance, int damage) : base("Attack") {
            this.maxDistance = maxDistance;
            this.damage = damage;
        }
    }
}