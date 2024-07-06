using DefaultNamespace;
using Objects.Abilities.Ground_Slash;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Geode_Attack
{
    public class GeodeProjectile : PoolableProjectile<GeodeProjectile>
    {
        public void OnEnemyHit(Damageable damageable)
        {
            SimpleDamage(damageable, false, false);
        }
    }
}