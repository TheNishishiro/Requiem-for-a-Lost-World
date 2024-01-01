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
                damageable.ApplyDamageOverTime(WeaponStats.GetDamage() * 0.5f, WeaponStats.GetDamageOverTimeFrequency(), WeaponStats.GetDamageOverTimeDuration(), ParentWeapon);
            }
        }
    }
}