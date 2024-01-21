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
    public class WindSheerWeapon : PoolableWeapon<WindShearProjectile>
    {
        public bool IsHurricaneWrath;

        protected override bool ProjectileSpawn(WindShearProjectile projectile)
        {
            var enemy = EnemyManager.instance.GetRandomEnemy();
            if (enemy == null)
                return false;

            projectile.transform.position = enemy.TargetPoint.position;
            projectile.SetParentWeapon(this);
            return true;
        }

        protected override void OnLevelUp()
        {
            if (LevelField == 9)
                IsHurricaneWrath = true;
        }
    }
}