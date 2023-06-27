using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Environment;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.LightningStrike
{
	public class LightningStrikeWeapon : WeaponBase
	{
		[SerializeField] public GameObject chainLightningPrefab;
		[HideInInspector] public bool IsChainLightning;
		
		public override void Attack()
		{
			var enemy = FindObjectsOfType<Damageable>().OrderBy(x => Random.value).FirstOrDefault();
			if (enemy == null) return;
			
			var lightningStrike = SpawnManager.instance.SpawnObject(enemy.transform.position, spawnPrefab);
			var projectileComponent = lightningStrike.GetComponent<LightningStrikeProjectile>();

			projectileComponent.SetParentWeapon(this);
			projectileComponent.SetStats(weaponStats);
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