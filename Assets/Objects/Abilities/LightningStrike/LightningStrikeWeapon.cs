using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Characters;
using Objects.Environment;
using Objects.Stage;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.LightningStrike
{
	public class LightningStrikeWeapon : PoolableWeapon<LightningStrikeProjectile>
	{
		[SerializeField] public GameObject chainLightningPrefab;
		[HideInInspector] public bool IsChainLightning;

		protected override bool ProjectileSpawn(LightningStrikeProjectile projectile)
		{
			var enemy = FindObjectsByType<Damageable>(FindObjectsSortMode.None).OrderBy(x => Random.value).FirstOrDefault();
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