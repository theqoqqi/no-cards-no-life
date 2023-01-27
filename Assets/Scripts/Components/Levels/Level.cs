using Components.Boards;
using Components.Entities;
using Components.Entities.Parts;
using Core.Events;
using Core.Events.Levels;
using UnityEngine;

namespace Components.Levels {
    public class Level : MonoBehaviour {

        private Board board;

        private void Start() {
            GameEvents.Instance.Dispatch<LevelLoadedEvent>(e => {
                e.Setup(this);
            });
        }

        private void OnEnable() {
            SetBoard(GetComponentInChildren<Board>());
            
            GameEvents.Instance.On<EntityDestroyedEvent>(OnEntityDestroyed);
        }

        private void OnDisable() {
            SetBoard(null);
            
            GameEvents.Instance.Off<EntityDestroyedEvent>(OnEntityDestroyed);
        }

        private void SetBoard(Board board) {
            if (this.board) {
                RemoveBoardListeners();
            }
            
            this.board = board;

            if (this.board) {
                AddBoardListeners();
            }
        }

        private void RemoveBoardListeners() {
            board.Player.Killed -= OnPlayerKilled;
        }

        private void AddBoardListeners() {
            board.Player.Killed += OnPlayerKilled;
        }

        private void OnEntityDestroyed(EntityDestroyedEvent e) {
            if (e.Entity.GetComponent<Enemy>() && !board.HasEnemies()) {
                OnLevelDone();
            }
        }

        private void OnPlayerKilled(Health.DamageDetails damageDetails) {
            GameEvents.Instance.Dispatch<PlayerDiedEvent>(e => {
                e.Setup(this);
            });
        }

        private void OnLevelDone() {
            GameEvents.Instance.Dispatch<LevelDoneEvent>(e => {
                e.Setup(this);
            });
        }
    }
}