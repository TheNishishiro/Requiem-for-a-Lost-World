using DefaultNamespace;
using Weapons;

namespace Objects.Abilities.Lightning_Needle
{
    public class LightningNeedleWeapon : PoolableWeapon<LightningNeedleProjectile>
    {
        private float _currentOffset;
        private float _offsetStep;
        public bool StormSurge { get; set; }

        protected override bool ProjectileSpawn(LightningNeedleProjectile projectile)
        {
            projectile.transform.position = Utilities.GetRandomInAreaFreezeParameter(transform.position, 0.2f, isFreezeZ: true);
            projectile.SetDirection(transform.forward, _currentOffset);
            _currentOffset += _offsetStep;
            projectile.SetParentWeapon(this);
            return true;
        }

        protected override void OnAttackStart()
        {
            _currentOffset = -45;
            _offsetStep = 90.0f / GetAttackCount();
            
            base.OnAttackStart();
        }

        protected override void OnLevelUp()
        {
            if (LevelField == 9)
                StormSurge = true;
        }
    }
}