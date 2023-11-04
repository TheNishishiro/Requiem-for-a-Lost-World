using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;

namespace Objects.Abilities.SpaceExpansionBall
{
	public class SpaceBallWeapon : PoolableWeapon<SpaceBallProjectile>
	{
		[HideInInspector] public bool IsGallacticCollapse;
		[SerializeField] public GameObject ExplosionPrefab;

		private ObjectPool<SimpleDamageProjectile> _subProjectilePool;
		private Vector3 _subProjectilePosition;

		public override void Awake()
		{
			base.Awake();

			var subProjectileStats = new WeaponStats()
			{
				TimeToLive = 0.5f
			};
			_subProjectilePool = new ObjectPool<SimpleDamageProjectile>(
				() =>
				{
					var projectile = SpawnManager.instance.SpawnObject(_subProjectilePosition, ExplosionPrefab).GetComponent<SimpleDamageProjectile>();
					projectile.Init(_subProjectilePool, projectile);
					return projectile;
				},
				projectile =>
				{
					subProjectileStats.Damage = weaponStats.GetDamage() * 2;
					subProjectileStats.Scale = weaponStats.GetScale();
					projectile.SetStats(subProjectileStats);
					projectile.SetParentWeapon(this);
					projectile.gameObject.SetActive(true);
				},
				projectile => projectile.gameObject.SetActive(false),
				projectile => Destroy(projectile.gameObject),
				true, 150, 200
			);
		}
		
		public void SpawnSubProjectile(Vector3 position)
		{
			_subProjectilePosition = position;
			_subProjectilePool.Get();
		}
		
		protected override bool ProjectileSpawn(SpaceBallProjectile projectile)
		{
			var enemy = EnemyManager.instance.GetRandomEnemy();
			if (enemy == null)
				return false;
			
			var position = enemy.TargetPoint.position;

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