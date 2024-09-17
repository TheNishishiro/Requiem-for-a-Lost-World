using Data.Elements;
using Objects.Enemies;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Low_Pressure
{
    public class LowPressureProjectile : PoolableProjectile<LowPressureProjectile>
    {
        private LowPressureWeapon LowPressureWeapon => (LowPressureWeapon)ParentWeapon;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy"))
                return;

            var enemy = other.GetComponent<Enemy>();

            var damageable = enemy.GetDamagableComponent();
            SimpleFollowUpDamage(damageable);
            if (LowPressureWeapon.IsReduceWindResOnHit)
                damageable.ReduceElementalDefence(Element.Wind, 0.15f);
            if (LowPressureWeapon.IsSlowOnHit)
                enemy.GetChaseComponent().SetSlow(2, 0.2f);
        }
    }
}