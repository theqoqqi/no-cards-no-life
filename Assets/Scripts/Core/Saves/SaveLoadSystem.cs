
using System.IO;
using Core.GameStates;
using UnityEngine;

namespace Core.Saves {
    public static class SaveLoadSystem {

        private const string SaveFileName = "savegame.json";

        private static readonly string SaveFilePath = Application.persistentDataPath + "/" + SaveFileName;
        
        public static bool HasSave => File.Exists(SaveFilePath);

        public static void SaveGame(GameState gameState) {
            var saveData = Save(gameState);
            
            SaveToFile(saveData);
        }

        public static GameState LoadGame() {
            var saveData = LoadFromFile();
            
            return Load(saveData);
        }

        public static void DeleteSave() {
            File.Delete(SaveFilePath);
        }

        private static void SaveToFile(GameSaveData saveData) {
            var jsonText = JsonUtility.ToJson(saveData);
            
            File.WriteAllText(SaveFilePath, jsonText);
        }

        private static GameSaveData LoadFromFile() {
            var jsonText = File.ReadAllText(SaveFilePath);
            
            return JsonUtility.FromJson<GameSaveData>(jsonText);
        }

        private static GameSaveData Save(GameState gameState) {
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