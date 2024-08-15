using DefaultNamespace;
using Weapons;

namespace Objects.Abilities.Wind_Succ
{
    public class WindSuccWeapon : PoolableWeapon<WindSuccProjectile>
    {
        public override void SetupProjectile(NetworkProjectile networkProjectile)
        {
            var position = Utilities.GetPointOnColliderSurface(Utilities.GetRandomInArea(transform.position, 5), transform);
            networkProjectile.Initialize(this, position);
        }
    }
}