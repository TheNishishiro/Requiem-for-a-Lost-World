using DefaultNamespace;
using Weapons;

namespace Objects.Abilities.Pillar_Of_Despair
{
    public class PillarOfDespairWeapon : PoolableWeapon<PillarOfDespairProjectile>
    {
        public bool VoidImpact1 { get; set; }
        public bool VoidImpact2 { get; set; }
        
        protected override void SetWeaponStatsStrategy()
        {
            WeaponStatsStrategy = new PillarOfDespairStrategy(this);
        }

        public override void SetupProjectile(NetworkProjectile networkProjectile)
        {
            var position = Utilities.GetPointOnColliderSurface(
                Utilities.GetRandomInArea(transform.position, 4f), transform
            );
            networkProjectile.Initialize(this, position);
        }

        protected override void OnLevelUp()
        {
            if (LevelField == 8)
                VoidImpact1 = true;
            if (LevelField == 9)
                VoidImpact2 = true;
        }
    }
}