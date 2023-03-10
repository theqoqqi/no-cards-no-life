using System;
using System.Threading.Tasks;
using Components.Entities.Parts;
using Core.Events;
using Core.Events.Levels;
using UnityEngine;

namespace Components.Entities {
    public class BaseEntity : EntityBehaviour {

        public event Action<Health.DamageDetails> Killed;

        [SerializeField] private Health health;

        public Health Health => health;
        
        [SerializeField] private GridAlignedBody body;

        public GridAlignedBody Body => body;

        public virtual Task StartAttack(BaseEntity victim, int damage) {
            victim.health.ApplyDamage(this, damage);
            
            return Task.CompletedTask;
        }

        private void OnEnable() {
            Health.EntityKilled.AddListener(OnKilled);
        }

        private void OnDisable() {
            Health.EntityKilled.RemoveListener(OnKilled);
        }

        private void OnDestroy() {
            GameEvents.Instance.Enqueue<EntityDestroyedEvent>().With(this);
        }

        private void OnKilled(Health.DamageDetails damageDetails) {
            Killed?.Invoke(damageDetails);
            
            Destroy(gameObject);
        }
    }
}