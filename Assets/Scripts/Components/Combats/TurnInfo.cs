namespace Components.Combats {
    public struct TurnInfo {

        public int turn;

        public int round;

        public int turnInRound;

        public TurnInfo(int turn, int round, int turnInRound) {
            this.turn = turn;
            this.round = round;
            this.turnInRound = turnInRound;
        }

        public TurnInfo Clone() {
            return new TurnInfo(turn, round, turnInRound);
        }
    }
}