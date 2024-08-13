using DefaultNamespace.Data.Game;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Enemies.EnemyWeapons
{
    public class EnemyWeapon<T> : PoolableWeapon<T> where T : PoolableProjectile<T>
    {
        protected override void SetWeaponStatsStrategy()
        {
            
        }

        public override void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer >= 0f || PauseManager.instance.IsPauseStateSet(GamePauseStates.PauseEnemyMovement)) return;

            _timer = WeaponStatsStrategy.GetTotalCooldown();
            CustomUpdate();
            StartCoroutine(AttackProcess());
        }
    }
}