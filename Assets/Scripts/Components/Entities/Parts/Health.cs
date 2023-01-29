using UnityEngine;
using UnityEngine.Events;

namespace Components.Entities.Parts {
    public class Health : MonoBehaviour {

        public readonly UnityEvent<DamageDetails> EntityKilled = new UnityEvent<DamageDetails>();

        public readonly UnityEvent<int> HitPointsChanged = new UnityEvent<int>();

        private BaseEntity owner;

        private int hitPoints;

        [SerializeField] private int maxHitPoints;

        [SerializeField] private bool isInvulnerable;

        public int MaxHitPoints {
            get => maxHitPoints;
            set => maxHitPoints = value;
        }

        public bool IsInvulnerable {
            get => isInvulnerable;
            set => isInvulnerable = value;
        }

        public bool IsFull => HitPoints >= MaxHitPoints;

        public float Percent {
            get => (float) HitPoints / MaxHitPoints;
            set => HitPoints = Mathf.CeilToInt(MaxHitPoints * value);
        }

        public int HitPoints {
            get => hitPoints;
            set => hitPoints = Mathf.Clamp(value, IsInvulnerable ? HitPoints : 0, IsAlive ? MaxHitPoints : 0);
        }

        public bool IsAlive => hitPoints > 0;

        private void Awake() {
            if (hitPoints == 0) {
                hitPoints = maxHitPoints;
            }

            owner = GetComponent<BaseEntity>();
        }

        public void Kill(BaseEntity attacker) {
            ApplyDamage(attacker, int.MaxValue);
        }

        public void ApplyDamage(BaseEntity attacker, int damage) {
            var damageDetails = new DamageDetails(attacker, owner, damage);
            
            ApplyDamage(damageDetails);
        }

        private void ApplyDamage(DamageDetails details) {
            ModifyHitPoints(-details.Damage);

            if (!IsAlive) {
                EntityKilled.Invoke(details);
            }
        }

        public void ApplyHealing(int healBy) {
            ModifyHitPoints(healBy);
        }

        private void ModifyHitPoints(int modifyBy) {
            var oldHitPoints = HitPoints;
            
            HitPoints += modifyBy;

            var modifiedBy = HitPoints - oldHitPoints;

            HitPointsChanged.Invoke(modifiedBy);
        }

        public readonly struct DamageDetails {

            public readonly BaseEntity Attacker;

            public readonly BaseEntity Victim;

            public readonly int Damage;

            public DamageDetails(BaseEntity attacker, BaseEntity victim, int damage) {
                Attacker = attacker;
                Victim = victim;
                Damage = damage;
            }
        }
    }
}