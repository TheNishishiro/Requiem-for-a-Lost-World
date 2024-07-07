using System.Collections;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Weapons;

namespace Objects.Abilities.Reality_Crack
{
	public class RealityShatterWeapon : PoolableWeapon<RealityShatterProjectile>
	{
		public bool IsGlobalDamage;
		public bool IsSelfBuff;
		
		protected override void SetWeaponStatsStrategy()
		{
			WeaponStatsStrategy = new RealityShatterStrategy(this);
		}
		
		public override void SetupProjectile(NetworkProjectile networkProjectile)
		{
			networkProjectile.Initialize(this, transform.position);
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