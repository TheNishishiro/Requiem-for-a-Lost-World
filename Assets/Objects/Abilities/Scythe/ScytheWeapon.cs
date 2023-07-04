using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Players.Scripts;
using Weapons;

namespace Objects.Abilities.Scythe
{
	public class ScytheWeapon : WeaponBase
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

		public override void Attack()
		{
			var katanaSlash = Instantiate(spawnPrefab, transform);
			var projectileComponent = katanaSlash.GetComponent<ScytheProjectile>();
			
			projectileComponent.SetParentWeapon(this);
			projectileComponent.SetPlayerHealthComponent(_healthComponent);
			projectileComponent.SetStats(weaponStats);
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
			if (LevelField == 5)
				IsBloodEmbrace = true;
			if (LevelField == 7)
				IsCursedStrikes = true;
			if (LevelField == 10)
				IsSoulHarvest = true;
		}
	}
}