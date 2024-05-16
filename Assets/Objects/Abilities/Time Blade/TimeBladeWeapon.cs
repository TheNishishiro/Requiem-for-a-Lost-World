using System.Collections;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Time_Blade
{
    public class TimeBladeWeapon : PoolableWeapon<TimeBladeProjectile>
    {
        public bool IsTemporalMastery { get; set; }
        private bool _isMinuteTick;

        public override void SetupProjectile(NetworkProjectile networkProjectile)
        {
            var currentScaleModifier = WeaponStatsStrategy.GetScale() * (_isMinuteTick ? 0.5f : 1f);

            var projectileTransform = networkProjectile.transform;
            networkProjectile.Initialize(this, transform.position);
            networkProjectile.ParentToPlayer();
            projectileTransform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            projectileTransform.localScale = new Vector3(currentScaleModifier,currentScaleModifier,currentScaleModifier);
            
            _isMinuteTick = !_isMinuteTick;
        }

        protected override IEnumerator AttackProcess()
        {
            OnAttackStart();
            for (var i = 0; i < GetAttackCount(); i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    Attack();
                }
                yield return new WaitForSeconds(WeaponStatsStrategy.GetDuplicateSpawnDelay());
            }
            OnAttackEnd();
        }

        protected override void OnLevelUp()
        {
            if (LevelField == 11)
                IsTemporalMastery = true;
        }
    }
}