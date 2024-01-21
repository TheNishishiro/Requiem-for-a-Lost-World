using System;
using Interfaces;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Back_Hole
{
	public class BlackHoleProjectile : PoolableProjectile<BlackHoleProjectile>
	{
		[SerializeField] private BlackHoleCenterComponent blackHoleCenter;
		[SerializeField] private BlackHoleGravitationWavesComponent blackHoleGravitation;
		
		public override void SetStats(IWeaponStatsStrategy weaponStatsStrategy)
		{
			base.SetStats(weaponStatsStrategy);
			blackHoleCenter.SetParentWeapon(ParentWeapon);
			blackHoleGravitation.SetParentWeapon(ParentWeapon);
		}
	}
}