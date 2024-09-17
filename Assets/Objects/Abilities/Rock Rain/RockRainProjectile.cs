using UnityEngine;
using Weapons;

namespace Objects.Abilities.Rock_Rain
{
    public class RockRainProjectile : PoolableProjectile<RockRainProjectile>
    {
        private Vector3 direction;
		
        public void SetDirection(float dirX, float dirY, float dirZ)
        {
            direction = (new Vector3(dirX, dirY, dirZ) - transformCache.position).normalized;
        }

        protected override void CustomUpdate()
        {
            transformCache.position += direction * ((WeaponStatsStrategy?.GetSpeed()).GetValueOrDefault() * Time.deltaTime);
            var ray = new Ray(transformCache.position, Vector3.down);
            var layer = LayerMask.GetMask("FloorLayer");
            var raycastResult = Physics.Raycast(ray, out var hit, Mathf.Infinity, layer);
				
            if (raycastResult && hit.distance < 0.5f)
            {
                SetState(ProjectileState.Dissipating);
            }
            
            transformCache.Rotate(Vector3.right * Random.Range(60f, 600f) * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            SimpleDamage(other, false, false, out var damageable);
            if (WeaponStatsStrategy.GetWeakness() > 0)
                damageable.SetVulnerable((WeaponEnum)ParentWeapon.GetId(), 2, WeaponStatsStrategy.GetWeakness());
        }
    }
}