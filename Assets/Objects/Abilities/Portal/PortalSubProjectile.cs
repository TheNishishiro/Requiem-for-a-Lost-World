using UnityEngine;
using Weapons;

namespace Objects.Abilities.Portal
{
    public class PortalSubProjectile : PoolableProjectile<PortalSubProjectile>
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
            SimpleFollowUpDamage(other);
            if (ParentWeapon.GetWeaponStrategy().GetDamageOverTime() > 0)
                DamageOverTime(other);
        }
    }
}