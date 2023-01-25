using System.Threading.Tasks;
using Components.Entities.Parts;
using UnityEngine;

namespace Components.Entities {
    public class BaseEntity : EntityBehaviour {

        [SerializeField] private Health health;

        public Health Health => health;
        
        [SerializeField] private GridAlignedBody body;

        public GridAlignedBody Body => body;

        public virtual Task StartAttack(BaseEntity victim, int damage) {
            var damageDetails = new Health.DamageDetails(this, damage);

            victim.health.ApplyDamage(damageDetails);
            
            return Task.CompletedTask;
        }
    }
}