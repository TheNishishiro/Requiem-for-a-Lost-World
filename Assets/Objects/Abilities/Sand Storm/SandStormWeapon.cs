using System;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Abilities.Magic_Ball;
using Objects.Enemies;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Wind_Shear
{
    public class SandStormWeapon : PoolableWeapon<SandStormProjectile>
    {
        public bool IsSuffocatingGrit;

        public override void SetupProjectile(NetworkProjectile networkProjectile)
        {
            var enemy = EnemyManager.instance.GetRandomEnemy();
            if (enemy == null)
            {
                networkProjectile.Despawn(WeaponId);
                return;
            }

            networkProjectile.Initialize(this, enemy.TargetPoint.position);
        }
        
        protected override void OnLevelUp()
        {
            if (LevelField == 9)
                IsSuffocatingGrit = true;
        }
    }
}