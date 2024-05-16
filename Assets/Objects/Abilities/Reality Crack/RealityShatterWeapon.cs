using System.Collections;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Weapons;

namespace Objects.Abilities.Reality_Crack
{
	public class RealityShatterWeapon : WeaponBase
	{
		public bool IsGlobalDamage;
		public bool IsSelfBuff;
		
		public override void Attack()
		{
			var shatterDome = SpawnManager.instance.SpawnObject(transform.position, spawnPrefab);
			var projectileComponent = shatterDome.GetComponent<RealityShatterProjectile>();

			projectileComponent.SetParentWeapon(this);
			//projectileComponent.SetStats();
		}

		protected override int GetAttackCount()
		{
			return 1;
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 9)
			{
				IsSelfBuff = true;
			}
			if (LevelField == 10)
			{
				IsGlobalDamage = true;
			}
		}

		public void IncreaseDamage()
		{
			weaponStats.DamageIncreasePercentage += 0.003f;
		}
	}
}