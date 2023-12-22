using System.Collections;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Time_Blade
{
    public class TimeBladeWeapon : PoolableWeapon<TimeBladeProjectile>
    {
        private bool _isMinuteTick;
        
        protected override TimeBladeProjectile ProjectileInit()
        {
            var timeBladeProjectile = Instantiate(spawnPrefab, transform).GetComponent<TimeBladeProjectile>();
            timeBladeProjectile.SetParentWeapon(this);
            return timeBladeProjectile;
        }
        
        protected override bool ProjectileSpawn(TimeBladeProjectile projectile)
        {
            var currentScaleModifier = weaponStats.GetScale() * (_isMinuteTick ? 0.5f : 1f);
            projectile.SetStats(weaponStats);

            var projectileTransform = projectile.transform;
            projectileTransform.position = transform.position;
            projectileTransform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            projectileTransform.localScale = new Vector3(currentScaleModifier,currentScaleModifier,currentScaleModifier);

            projectile.gameObject.SetActive(true);
            return true;
        }
        
        protected override IEnumerator AttackProcess()
        {
            for (var i = 0; i < GetAttackCount(); i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    Attack();
                    _isMinuteTick = !_isMinuteTick;
                }
                yield return new WaitForSeconds(weaponStats.DuplicateSpawnDelay);
            }
        }
    }
}