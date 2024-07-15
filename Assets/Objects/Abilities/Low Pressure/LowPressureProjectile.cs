using UnityEngine;
using Weapons;

namespace Objects.Abilities.Low_Pressure
{
    public class LowPressureProjectile : PoolableProjectile<LowPressureProjectile>
    {
        private void OnTriggerEnter(Collider other)
        {
            SimpleFollowUpDamage(other);
        }
    }
}