using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Abilities.Magic_Ball;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Throwing_Knife
{
	public class ThrowingKnifeWeapon : WeaponBase
	{
		public override void Attack()
		{
			var playerTransform = FindObjectsOfType<Player>().FirstOrDefault();
			
			var throwingKnife = SpawnManager.instance.SpawnObject(
				Utilities.GetRandomInAreaFreezeParameter(transform.position, 0.2f, isFreezeZ: true)
				, spawnPrefab);
			var projectileComponent = throwingKnife.GetComponent<ThrowingKnifeProjectile>();
			
			projectileComponent.SetParentWeapon(this);
			projectileComponent.SetDirection(playerTransform.transform.forward);
			projectileComponent.SetStats(weaponStats);
		}

		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Survive15MinutesWithEliza);
		}
	}
}