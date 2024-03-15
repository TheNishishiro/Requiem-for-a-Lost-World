using DefaultNamespace;
using Weapons;

namespace Objects.Abilities.Light_Pillars
{
    public class LightPillarWeapon : PoolableWeapon<LightPillarProjectile>
    {
        public bool IsDivineBarrage { get; set; }

        public override void SetupProjectile(NetworkProjectile networkProjectile)
        {
            var position = Utilities.GetPointOnColliderSurface(
                Utilities.GetRandomInArea(transform.position, 4f), transform
            );
            networkProjectile.Initialize(this, position);
        }

        protected override void OnLevelUp()
        {
            if (LevelField == 11)
                IsDivineBarrage = true;
        }
    }
}