using System.Collections;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Enemies;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.BindingField
{
	public class BindingFieldWeapon : WeaponBase
	{
		public double ChanceToBind { get; private set; } = 0.5f;
		public bool IsBurstDamage { get; private set; }

		public override void Attack()
		{
			var randomEnemy = FindObjectsOfType<Enemy>().OrderBy(x => Random.value).FirstOrDefault();
			if (randomEnemy == null)
				return;

			var pointOnSurface = Utilities.GetPointOnColliderSurface(new Vector3(randomEnemy.transform.position.x, 0, randomEnemy.transform.position.z), transform);
			var bindingField = SpawnManager.instance.SpawnObject(pointOnSurface, spawnPrefab);
			var projectileComponent = bindingField.GetComponent<BindingFieldProjectile>();

			projectileComponent.SetParentWeapon(this);
			projectileComponent.SetStats(weaponStats);
		}

		protected override IEnumerator AttackProcess()
		{
			Attack();
			yield break;
		}

		protected override void OnLevelUp()
		{
			switch (LevelField)
			{
				case 6:
					IsBurstDamage = true;
					break;
				case 7:
					ChanceToBind = 0.8f;
					break;
			}
		}

		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Survive15MinutesWithAmelisana);
		}
	}
}