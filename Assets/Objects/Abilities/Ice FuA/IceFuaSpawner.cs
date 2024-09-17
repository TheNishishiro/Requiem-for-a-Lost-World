using System;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data.Achievements;
using Events.Handlers;
using Events.Scripts;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Ice_FuA
{
    public class IceFuaSpawner : PoolableProjectile<IceFuaSpawner>, IExpPickedUpHandler
    {
        private IceFuaWeapon IceFuaWeapon => (IceFuaWeapon)ParentWeapon;
        private int _expShardPickedUp;
        private float _fireRate;

        private void OnEnable()
        {
            ExpPickedUpEvent.Register(this);
        }

        private void OnDisable()
        {
            ExpPickedUpEvent.Unregister(this);
        }

        protected override void CustomUpdate()
        {
            _fireRate -= Time.deltaTime;
            if (_fireRate < 0)
            {
                var target = Utilities.GetEnemiesInAreaNonAlloc(transformCache.position, 5).FirstOrDefault();
                if (target)
                    IceFuaWeapon.SpawnSubProjectile(transformCache.position, target);

                _fireRate = WeaponStatsStrategy.GetDuplicateSpawnDelay();
            }
            
            if (TimeAlive > 30)
                AchievementManager.instance.UnlockAchievement(AchievementEnum.KeepFrostSeedUpFor30Seconds);
        }

        public void OnExpPickedUp(float amount)
        {
            CurrentTimeToLive += 0.1f;
        }
    }
}