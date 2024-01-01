using Objects.Abilities.Ice_Wave;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Time_Blade
{
    public class TimeBladeProjectile : PoolableProjectile<TimeBladeProjectile>
    {
        private TimeBladeWeapon TimeBladeWeapon => (TimeBladeWeapon)ParentWeapon;
        
        private void OnTriggerEnter(Collider other)
        {
            SimpleDamage(other, false);
            var chaseComponent = other.GetComponentInParent<ChaseComponent>();
            if (chaseComponent == null) return;
            
            var immobileTime = TimeBladeWeapon.IsTemporalMastery ? 0.6f : 0.25f;
            chaseComponent.SetImmobile(immobileTime);
        }
    }
}