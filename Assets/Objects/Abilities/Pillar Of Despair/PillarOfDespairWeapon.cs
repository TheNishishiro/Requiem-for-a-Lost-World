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

        protected override bool ProjectileSpawn(PillarOfDespairProjectile projectile)
        {
            projectile.transform.position = Utilities.GetPointOnColliderSurface(
                Utilities.GetRandomInArea(transform.position, 3f), transform
            );
            projectile.SetParentWeapon(this);
            return true;
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