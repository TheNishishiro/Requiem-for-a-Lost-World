using Objects.Abilities.Ice_Wave;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Time_Blade
{
    public class TimeBladeProjectile : PoolableProjectile<TimeBladeProjectile>
    {
        private void OnTriggerEnter(Collider other)
        {
            SimpleDamage(other, false);
            var chaseComponent = other.GetComponentInParent<ChaseComponent>();
            if (chaseComponent != null)
            {
                chaseComponent.SetImmobile(0.25f);
            }
        }
    }
}