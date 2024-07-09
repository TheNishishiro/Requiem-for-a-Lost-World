using DefaultNamespace;
using Weapons;

namespace Objects.Abilities.Geode_Attack
{
    public class GeodeWeapon : PoolableWeapon<GeodeProjectile>
    {
        public bool IsSeismicResonance { get; set; }
        
        public override void SetupProjectile(NetworkProjectile networkProjectile)
        {
            var position = Utilities.GetPointOnColliderSurface(
                Utilities.GetRandomInArea(transform.position, 5f), transform
            );
            networkProjectile.Initialize(this, position);
        }

        protected override void OnLevelUp()
        {
            if (LevelField == 9)
                IsSeismicResonance = true;
        }
    }
}