using UnityEngine;
using UnityEngine.Events;

namespace Components.Entities.Parts {
    public class Health : MonoBehaviour {

        public readonly UnityEvent<DamageDetails> OnEntityKilled = new UnityEvent<DamageDetails>();

        public readonly UnityEvent<int> OnHitPointsChanged = new UnityEvent<int>();

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
        }

        public void ApplyDamage(DamageDetails details) {
            ModifyHitPoints(-details.Damage);

            if (!IsAlive) {
                OnEntityKilled.Invoke(details);
            }
        }

        public void ApplyHealing(int healBy) {
            ModifyHitPoints(healBy);
        }

        private void ModifyHitPoints(int modifyBy) {
            HitPoints += modifyBy;

            OnHitPointsChanged.Invoke(modifyBy);
        }

        public void Kill(BaseEntity attacker) {
            var details = new DamageDetails(attacker, int.MaxValue);

            ApplyDamage(details);
        }

        public readonly struct DamageDetails {

            public readonly BaseEntity Attacker;

            public readonly int Damage;

            public DamageDetails(BaseEntity attacker, int damage) {
                Attacker = attacker;
                Damage = damage;
            }
        }
    }
}