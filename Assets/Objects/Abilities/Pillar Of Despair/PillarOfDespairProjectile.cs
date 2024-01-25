using System.Drawing;
using Interfaces;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Pillar_Of_Despair
{
    public class PillarOfDespairProjectile : PoolableProjectile<PillarOfDespairProjectile>
    {
        private PillarOfDespairWeapon PillarOfDespairWeapon => (PillarOfDespairWeapon)ParentWeapon;

        private void OnTriggerStay(Collider other)
        {
            DamageArea(other, out var damageable);
            if (damageable == null || damageable.IsDestroyed()) return;
            
            if (WeaponStatsStrategy.GetWeakness() > 0)
                damageable?.SetVulnerable(2f, WeaponStatsStrategy.GetWeakness());
            if (WeaponStatsStrategy.GetDamageOverTime() > 0)
                damageable.ApplyDamageOverTime(WeaponStatsStrategy.GetDamageOverTime(), WeaponStatsStrategy.GetDamageOverTimeFrequency(), WeaponStatsStrategy.GetDamageOverTimeDuration(), ParentWeapon);
        }
    }
}