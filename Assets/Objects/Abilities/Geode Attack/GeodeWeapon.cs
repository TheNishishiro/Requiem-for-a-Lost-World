using DefaultNamespace;
using Weapons;

namespace Objects.Abilities.Geode_Attack
{
    public class GeodeWeapon : PoolableWeapon<GeodeProjectile>
    {
        public override void SetupProjectile(NetworkProjectile networkProjectile)
        {
            var position = Utilities.GetPointOnColliderSurface(
                Utilities.GetRandomInArea(transform.position, 5f), transform
            );
            networkProjectile.Initialize(this, position);
        }
    }
}