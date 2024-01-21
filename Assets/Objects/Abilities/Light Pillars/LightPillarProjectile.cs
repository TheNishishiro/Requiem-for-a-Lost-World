using System;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Light_Pillars
{
    public class LightPillarProjectile : PoolableProjectile<LightPillarProjectile>
    {
        private LightPillarWeapon LightPillarWeapon => (LightPillarWeapon)ParentWeapon;
        
        private void OnTriggerStay(Collider other)
        {
            DamageArea(other, out var damageable);
            if (LightPillarWeapon.IsDivineBarrage && damageable != null && !damageable.IsDestroyed())
            {
                damageable.ApplyDamageOverTime(WeaponStatsStrategy.GetDamage() * 0.5f, WeaponStatsStrategy.GetDamageOverTimeFrequency(), WeaponStatsStrategy.GetDamageOverTimeDuration(), ParentWeapon);
            }
        }
    }
}