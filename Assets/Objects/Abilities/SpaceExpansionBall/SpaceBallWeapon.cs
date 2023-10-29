using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.SpaceExpansionBall
{
	public class SpaceBallWeapon : PoolableWeapon<SpaceBallProjectile>
	{
		[HideInInspector] public bool IsGallacticCollapse;
		[SerializeField] public GameObject ExplosionPrefab;

		protected override bool ProjectileSpawn(SpaceBallProjectile projectile)
		{
			var enemy = EnemyManager.instance.GetRandomEnemy();
			if (enemy == null)
				return false;
			
			var position = enemy.transform.position;

			projectile.transform.position = transform.position;
			projectile.SetStats(weaponStats);
			projectile.SetDirection(position.x, position.y, position.z);
			return true;
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 9)
				IsGallacticCollapse = true;
		}

		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Survive15MinutesWithAdam);
		}
	}
}