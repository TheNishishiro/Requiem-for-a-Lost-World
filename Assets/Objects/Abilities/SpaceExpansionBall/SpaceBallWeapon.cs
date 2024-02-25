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
		private WeaponStats _subProjectileStats;
		private WeaponStatsStrategyBase _subProjectileStatsStrategy;

		public override void Awake()
		{
			base.Awake();
			_subProjectileStats = new WeaponStats()
			{
				TimeToLive = 0.5f
			};
			_subProjectileStatsStrategy = new WeaponStatsStrategyBase(_subProjectileStats, ElementField);
			
			_subProjectilePool = new ObjectPool<SimpleDamageProjectile>(
				() =>
				{
					var projectile = SpawnManager.instance.SpawnObject(_subProjectilePosition, ExplosionPrefab).GetComponent<SimpleDamageProjectile>();
					projectile.Init(_subProjectilePool, projectile);
					return projectile;
				},
				projectile =>
				{
					_subProjectileStats.Damage = WeaponStatsStrategy.GetDamage() * 2;
					_subProjectileStats.Scale = WeaponStatsStrategy.GetScale();
					projectile.transform.position = _subProjectilePosition;
					projectile.SetParentWeapon(this, false);
					projectile.SetStats(_subProjectileStatsStrategy);
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
			projectile.SetParentWeapon(this);
			projectile.SetDirection(position.x, position.y, position.z);
			return true;
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 9)
				IsGallacticCollapse = true;
		}
	}
}