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
    public class WindSheerWeapon : WeaponBase
    {
        public bool IsHurricaneWrath;
        
        public override void Attack()
        {
            var enemy = FindObjectsByType<Enemy>(FindObjectsSortMode.None).OrderBy(_ => Random.value).FirstOrDefault();
            if (enemy == null)
                return;

            var windShear = SpawnManager.instance.SpawnObject(enemy.transform.position, spawnPrefab);
            var projectileComponent = windShear.GetComponent<WindShearProjectile>();
            projectileComponent.SetStats(weaponStats);
            projectileComponent.SetParentWeapon(this);
        }

        protected override void OnLevelUp()
        {
            if (LevelField == 9)
                IsHurricaneWrath = true;
        }

        public override bool IsUnlocked(SaveFile saveFile)
        {
            return saveFile.IsAchievementUnlocked(AchievementEnum.UnlockWindShear);
        }
    }
}