using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Abilities.BindingField;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Healing_Field
{
	public class HealingFieldWeapon : PoolableWeapon<HealingFieldProjectile>
	{
		[HideInInspector] public bool IsEmpowering;

		protected override bool ProjectileSpawn(HealingFieldProjectile projectile)
		{
			var pointOnSurface = Utilities.GetPointOnColliderSurface(new Vector3(transform.position.x, 0, transform.position.z), transform);
			projectile.transform.position = pointOnSurface;
			projectile.SetStats(weaponStats);
			return true;
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