using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Managers.StageEvents;
using Objects.Abilities.Lightning_Chain;
using Objects.Characters;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;

namespace Objects.Abilities.Tornado
{
	public class TornadoWeapon : PoolableWeapon<TornadoProjectile>
	{
		[SerializeField] public GameObject lightningChainPrefab;
		private StageTime _stageTime;
		public bool IsStaticDischarge { get; private set; }
		
		private ObjectPool<LightningChainProjectile> _subProjectilePool;
		private Vector3 _subProjectilePosition;
		
		public override void Awake()
		{
			_stageTime = FindFirstObjectByType<StageTime>();
			base.Awake();
			var subWeaponStats = new WeaponStats()
			{
				TimeToLive = 0.3f,
				Scale = 1f,
				DetectionRange = 2f
			};
			
			_subProjectilePool = new ObjectPool<LightningChainProjectile>(
				() =>
				{
					var projectile = SpawnManager.instance.SpawnObject(_subProjectilePosition, lightningChainPrefab).GetComponent<LightningChainProjectile>();
					projectile.Init(_subProjectilePool, projectile);
					projectile.SetParentWeapon(this);
					return projectile;
				},
				projectile =>
				{
					subWeaponStats.Damage = weaponStats.GetDamage() * 2.5f;
					projectile.SetStats(subWeaponStats);
					projectile.gameObject.SetActive(true);
					projectile.SeekTargets(2);
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

		protected override bool ProjectileSpawn(TornadoProjectile projectile)
		{
			var spawnRadius = GameData.GetPlayerCharacterId() == CharactersEnum.Natalie_BoW ? 
				Mathf.Lerp(1, 4, (float)Utilities.GetTimeSpan(_stageTime.time).TotalSeconds / 300.0f)  
				: 4;
			
			var tornadoPosition = Utilities.GetRandomInAreaFreezeParameter(transform.position, spawnRadius, isFreezeY:true);
			var pointOnSurface = Utilities.GetPointOnColliderSurface(tornadoPosition, transform);
			projectile.transform.position = pointOnSurface;
			projectile.gameObject.SetActive(true);
			projectile.SetStats(weaponStats);
			return true;
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 10)
				IsStaticDischarge = true;
		}

		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Survive15MinutesWithNatalie);
		}
	}
}