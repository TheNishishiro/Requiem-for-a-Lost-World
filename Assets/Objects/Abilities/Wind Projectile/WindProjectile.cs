using UnityEngine;
using Weapons;

namespace Objects.Abilities.Wind_Projectile
{
    public class WindProjectile : PoolableProjectile<WindProjectile>
    {
        private Vector3 _direction;
		
        protected override void CustomUpdate()
        {
            transformCache.position += _direction * (WeaponStatsStrategy.GetSpeed() * Time.deltaTime);
        }
		
        public void SetDirection(Vector3 dir)
        {
            _direction = dir;
            transformCache.rotation = Quaternion.LookRotation(_direction.normalized) * Quaternion.Euler(90,0,0);
        }

        private void OnTriggerEnter(Collider other)
        {
            SimpleDamage(other, true,false, out var damageable);
            DamageOverTime(damageable, other);
        }
    }
}