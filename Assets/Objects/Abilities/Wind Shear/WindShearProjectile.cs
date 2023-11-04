using System;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Wind_Shear
{
    public class WindShearProjectile : PoolableProjectile<WindShearProjectile>
    {
        public WindSheerWeapon WindShearWeapon => (WindSheerWeapon)ParentWeapon;
        
        private void Update()
        {
            TickLifeTime();
        }

        private void OnTriggerEnter(Collider other)
        {
            DamageOverTime(other);
            if (WindShearWeapon.IsHurricaneWrath)
            {
                var chaseComponent = other.GetComponent<ChaseComponent>();
                if (chaseComponent != null)
                    chaseComponent.SetSlow(0.3f, 1);
            }
        }
    }
}