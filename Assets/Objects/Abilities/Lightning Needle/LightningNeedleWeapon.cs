using DefaultNamespace;
using Weapons;

namespace Objects.Abilities.Lightning_Needle
{
    public class LightningNeedleWeapon : PoolableWeapon<LightningNeedleProjectile>
    {
        private float _currentOffset;
        private float _offsetStep;
        public bool StormSurge { get; set; }
        
        public override void SetupProjectile(NetworkProjectile networkProjectile)
        {
            var position = Utilities.GetRandomInAreaFreezeParameter(transform.position, 0.2f, isFreezeZ: true);
            networkProjectile.GetProjectile<LightningNeedleProjectile>().SetDirection(transform.forward, _currentOffset);
            networkProjectile.Initialize(this, position);
            _currentOffset += _offsetStep;
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