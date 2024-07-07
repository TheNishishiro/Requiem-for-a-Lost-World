using Weapons;

namespace Objects.Abilities.Magic_Arrow
{
    public class MagicArrowWeapon : PoolableWeapon<MagicArrowProjectile>
    {
        public override void SetupProjectile(NetworkProjectile networkProjectile)
        {
            var transform1 = transform;
            var projectilePosition = transform1.position + transform1.forward/2;
            var enemy = EnemyManager.instance.GetRandomEnemy();
            if (enemy == null)
            {
                networkProjectile.Despawn(WeaponId);
                return;
            }
            
            networkProjectile.Initialize(this, projectilePosition);
            var projectile = networkProjectile.GetProjectile<MagicArrowProjectile>();
            projectile.SetChaseTarget(enemy);
            projectile.ClearTrail();
            networkProjectile.projectile.gameObject.SetActive(true);
        }
    }
}