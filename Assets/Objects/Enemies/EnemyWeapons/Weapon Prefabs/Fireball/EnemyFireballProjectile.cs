using System;
using UnityEngine;

namespace Objects.Enemies.EnemyWeapons.Weapon_Prefabs
{
    public class EnemyFireballProjectile : EnemyProjectile<EnemyFireballProjectile>
    {
        private Vector3 _direction;

        public void SetDirection(float dirX, float dirY, float dirZ)
        {
            _direction = (new Vector3(dirX, dirY, dirZ) - transformCache.position).normalized;
        }

        protected override void CustomUpdate()
        {
            transformCache.position += _direction * ((WeaponStatsStrategy?.GetSpeed() ?? 0) * Time.deltaTime);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            SimpleDamage(other);
        }
    }
}