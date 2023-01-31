
using Core.GameStates;

namespace Core.Saves {
    public static class SaveLoadSystem {

        public static GameSaveData Save(GameState gameState) {
            return GameSaveData.Save(gameState);
        }

        public static GameState Load(GameSaveData saveData) {
            var gameState = GameSaveData.Load(saveData);

            return Repair(gameState);
        }

        private static GameState Repair(GameState gameState) {
            if (gameState.StarterDeck == null) {
                gameState.StartFirstRun();
            }

            if (gameState.CurrentRun == null) {
                gameState.StartNewRun();
            }

            return gameState;
        }
    }
}