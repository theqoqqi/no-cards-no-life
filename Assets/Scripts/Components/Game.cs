using System;
using System.Threading.Tasks;
using Components.Scenes;
using Core.Cards;
using Core.Events;
using Core.Events.Levels;
using Core.GameStates;
using UnityEngine;

namespace Components {
    public class Game : MonoBehaviour {
        
        /*
         * Наверное, игровые данные должны просто храниться в компоненте (ну, в каком-нибудь GameManager),
         * а система загрузки/сохранений уже просто напрямую из этого компонента берет необходимые данные и сохраняет.
         *
         * К примеру, в GameManager может храниться Deck CurrentDeck, List<Card> UnlockedCards, int deaths и т.д.
         * То есть это могут быть любые данные, включая мои кастомные классы. Так будет достигнуто следующее:
         * 1. Все данные хранятся в централизованном хранилище (GameManager)
         * 2. Можно свободно создавать иерархию собственных классов (Deck, Card и т.д.)
         * 3. Система загрузки/сохранений может просто обращаться к GameManager, из которого будет брать данные
         * 4. Использование системы можно будет свести к вызовам методов NewGame/SaveGame/LoadGame/ResetGame и т.д.
         *
         * Вроде бы получается достаточно чистая и минимально связанная архитектура.
         * 
         * Возможно еще понадобится добавить классы типа SaveGameData, чтобы отделить систему З/С от компонентов.
         * Тогда получится так: GameManager -> SaveGameData -> SaveLoadManager.
         * Это позволит отделить чтение данных из игры от записи данных в файл и наоборот.
         */

        public static Game Instance { get; private set; }

        public static bool IsBootstrapDone => Instance;

        public static event Action OnBootstrapDone;

        private static bool loadingFromBootstrap;

        public GameState GameState { get; } = new GameState();

        [SerializeField] private SceneManager sceneManager;

        public SceneManager SceneManager => sceneManager;

        private async void Awake() {
            if (Instance) {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;

            InitTestDeck();

            if (!loadingFromBootstrap) {
                await sceneManager.LoadMainMenu();
            }
            
            GameEvents.Instance.On<LevelDoneEvent>(async e => {
                await sceneManager.LoadLocationMap();
            });
            
            GameEvents.Instance.On<LevelFailedEvent>(async e => {
                await sceneManager.LoadDeathScreen();
            });
        }

        private void InitTestDeck() {
            var deck = new Deck();

            deck.AddCard(new MoveCard(1));
            deck.AddCard(new MoveCard(2));
            deck.AddCard(new MoveCard(3));
            deck.AddCard(new AttackCard(2, 1));
            deck.AddCard(new AttackCard(1, 2));
            deck.AddCard(new AttackCard(2, 2));
            deck.AddCard(new AttackCard(2, 3));

            // deck.Shuffle();

            GameState.CurrentHand = deck;
        }

        public static async Task<Game> Bootstrap() {
            loadingFromBootstrap = true;
            
            SceneManager.LoadSystemScene();

            while (!Instance) {
                await Task.Yield();
            }
            
            OnBootstrapDone?.Invoke();
            
            return Instance;
        }
    }
}