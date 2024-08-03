using DefaultNamespace;
using Weapons;

namespace Objects.Abilities.Wind_Projectile
{
    public class WindProjectileWeapon : PoolableWeapon<WindProjectile>
    {
        public override void SetupProjectile(NetworkProjectile networkProjectile)
        {
            var position = Utilities.GetRandomInAreaFreezeParameter(transform.position, 0.2f, isFreezeZ: true);
            networkProjectile.Initialize(this, position);
            networkProjectile.GetProjectile<WindProjectile>().SetDirection(transform.forward);
        }
    }
}