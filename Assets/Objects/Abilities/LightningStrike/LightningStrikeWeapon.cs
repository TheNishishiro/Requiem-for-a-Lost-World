using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Abilities.Lightning_Chain;
using Objects.Characters;
using Objects.Environment;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.LightningStrike
{
	public class LightningStrikeWeapon : PoolableWeapon<LightningStrikeProjectile>
	{
		[SerializeField] public GameObject chainLightningPrefab;
		[HideInInspector] public bool IsChainLightning;
		
		private ObjectPool<LightningChainProjectile> _subProjectilePool;
		private Vector3 _subProjectilePosition;

		public override void Awake()
		{
			base.Awake();

			var subProjectileStats = new WeaponStats()
			{
				TimeToLive = 0.5f,
				DetectionRange = 1f
			};
			_subProjectilePool = new ObjectPool<LightningChainProjectile>(
				() =>
				{
					var projectile = SpawnManager.instance.SpawnObject(_subProjectilePosition, chainLightningPrefab).GetComponent<LightningChainProjectile>();
					projectile.Init(_subProjectilePool, projectile);
					projectile.SetParentWeapon(this);
					return projectile;
				},
				projectile =>
				{
					subProjectileStats.Damage = weaponStats.GetDamage() * 0.25f;
					subProjectileStats.Scale = weaponStats.GetScale();
					projectile.SetStats(subProjectileStats);
					projectile.gameObject.SetActive(true);
					projectile.SeekTargets(5);
				},
				projectile => projectile.gameObject.SetActive(false),
				projectile => Destroy(projectile.gameObject),
				true, 20, 40
			);
		}
		
		public void SpawnSubProjectile(Vector3 position)
		{
			_subProjectilePosition = position;
			_subProjectilePool.Get();
		}

		protected override bool ProjectileSpawn(LightningStrikeProjectile projectile)
		{
			var enemy = EnemyManager.instance.GetRandomEnemy();
			if (enemy == null) return false;

			projectile.transform.position = enemy.transform.position;
			projectile.SetStats(weaponStats);
			return true;
		}

		protected override int GetAttackCount()
		{
			var attackCount = base.GetAttackCount();
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Alice_BoL &&
			    GameData.GetPlayerCharacterRank() >= CharacterRank.E1)
				attackCount += 2;

			return attackCount;
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 8)
				IsChainLightning = true;
		}

		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Survive15MinutesWithAlice);
		}
	}
}