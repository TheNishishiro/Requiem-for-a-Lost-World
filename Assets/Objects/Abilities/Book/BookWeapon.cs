using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Data.Weapons;
using Managers;
using Objects.Abilities.Magic_Ball;
using Objects.Abilities.SpaceExpansionBall;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;

namespace Objects.Abilities.Book
{
	public class BookWeapon : PoolableWeapon<BookProjectile>
	{
		private float _rotationStep = 0;
		private float _rotateOffset = 0;
		public bool IsShadowBurst;
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
					var projectile = SpawnManager.instance.SpawnObject(_subProjectilePosition, ExplosionPrefab)
						.GetComponent<SimpleDamageProjectile>();
					projectile.Init(_subProjectilePool, projectile);
					return projectile;
				},
				projectile =>
				{
					projectile.SetParentWeapon(this, false);
					_subProjectileStats.Damage = WeaponStatsStrategy.GetDamage() * 0.25f;
					_subProjectileStats.Scale = WeaponStatsStrategy.GetScale();
					projectile.SetStats(_subProjectileStatsStrategy);
					projectile.transform.position = _subProjectilePosition;
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

		public override void SetupProjectile(NetworkProjectile networkProjectile, WeaponPoolEnum weaponPoolId)
		{
			var position = transform.position + new Vector3(WeaponStatsStrategy.GetScale(),0,0);
			networkProjectile.Initialize(this, position);
			networkProjectile.transform.RotateAround(transform.position, Vector3.up, _rotateOffset);
			
			var bookProjectile = networkProjectile.GetProjectile<BookProjectile>();
			bookProjectile.SetTarget(gameObject);
			
			_rotateOffset += _rotationStep;
		}

		protected override IEnumerator AttackProcess()
		{
			OnAttackStart();
			_rotationStep = GetRotationByAttackCount();
			_rotateOffset = 0;
			
			for (var i = 0; i < WeaponStatsStrategy.GetAttackCount(); i++)
			{
				Attack();
			}

			OnAttackEnd();
			yield break;
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 10)
				IsShadowBurst = true;
		}
	}
}