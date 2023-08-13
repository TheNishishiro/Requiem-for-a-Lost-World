using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Abilities.BindingField;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Healing_Field
{
	public class HealingFieldWeapon : WeaponBase
	{
		[HideInInspector] public bool IsEmpowering;
		
		public override void Attack()
		{
			var pointOnSurface = Utilities.GetPointOnColliderSurface(new Vector3(transform.position.x, 0, transform.position.z), transform);
			var healingField = SpawnManager.instance.SpawnObject(pointOnSurface, spawnPrefab);
			var projectileComponent = healingField.GetComponent<HealingFieldProjectile>();

			projectileComponent.SetParentWeapon(this);
			projectileComponent.SetStats(weaponStats);
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 9)
				IsEmpowering = true;
		}

		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Survive15MinutesWithAmelia);
		}

		protected override int GetAttackCount()
		{
			return 1;
		}
	}
}