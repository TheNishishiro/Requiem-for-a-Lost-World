using UnityEngine;
using Weapons;

namespace Objects.Abilities.Lightning_Needle
{
    public class LightningNeedleProjectile : PoolableProjectile<LightningNeedleProjectile>
    {
        public void SetDirection(Vector3 dir, float rotationDegree)
        {
            transformCache.rotation = Quaternion.LookRotation(dir.normalized) * Quaternion.Euler(0,rotationDegree,0);
        }

        protected override void CustomUpdate()
        {
            transformCache.position += transformCache.forward * ((WeaponStatsStrategy?.GetSpeed() ?? 0) * Time.deltaTime);
            base.CustomUpdate();
        }

        private void OnTriggerEnter(Collider other)
        {
            DamageOverTime(other, true);
        }
    }
}