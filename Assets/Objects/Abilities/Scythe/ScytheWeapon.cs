using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Players.Scripts;
using Weapons;

namespace Objects.Abilities.Scythe
{
	public class ScytheWeapon : PoolableWeapon<ScytheProjectile>
	{
		private HealthComponent _healthComponent;
		public bool IsBloodEmbrace;
		public bool IsCursedStrikes;
		public bool IsSoulHarvest;

		public override void Awake()
		{
			_healthComponent = FindObjectOfType<HealthComponent>();
			base.Awake();
		}

		protected override ScytheProjectile ProjectileInit()
		{
			var katanaSlash = Instantiate(spawnPrefab, transform).GetComponent<ScytheProjectile>();
			katanaSlash.SetParentWeapon(this);
			katanaSlash.SetPlayerHealthComponent(_healthComponent);
			return katanaSlash;
		}

		protected override bool ProjectileSpawn(ScytheProjectile projectile)
		{
			projectile.transform.position = transform.position;
			projectile.SetStats(weaponStats);
			return true;
		}

		protected override int GetAttackCount()
		{
			return 1;
		}

		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Survive15MinutesWithCorina);
		}

		protected override void OnLevelUp()
		{
			switch (LevelField)
			{
				case 5:
					IsBloodEmbrace = true;
					break;
				case 7:
					IsCursedStrikes = true;
					break;
				case 10:
					IsSoulHarvest = true;
					break;
			}
		}
	}
}