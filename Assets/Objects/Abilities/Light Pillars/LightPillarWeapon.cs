using DefaultNamespace;
using Weapons;

namespace Objects.Abilities.Light_Pillars
{
    public class LightPillarWeapon : PoolableWeapon<LightPillarProjectile>
    {
        public bool IsDivineBarrage { get; set; }
        
        protected override bool ProjectileSpawn(LightPillarProjectile projectile)
        {
            projectile.transform.position = Utilities.GetPointOnColliderSurface(
                Utilities.GetRandomInArea(transform.position, 3f), transform
                );
            projectile.SetParentWeapon(this);
            return true;
        }

        protected override void OnLevelUp()
        {
            if (LevelField == 11)
                IsDivineBarrage = true;
        }
    }
}