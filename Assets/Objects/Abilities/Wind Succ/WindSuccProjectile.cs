using System;
using Objects.Enemies;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Wind_Succ
{
    public class WindSuccProjectile : PoolableProjectile<WindSuccProjectile>
    {
        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
			
            var enemyComponent = other.GetComponent<Enemy>();
            if (enemyComponent == null) return;
            
            enemyComponent.GetChaseComponent().SetTemporaryTarget(gameObject, 3f, 0.5f);
            enemyComponent.SetNoCollisions(0.5f);
            DamageArea(other, out _);
        }
    }
}