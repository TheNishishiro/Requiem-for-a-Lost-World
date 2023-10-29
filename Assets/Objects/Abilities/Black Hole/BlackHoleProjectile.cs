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
		
		public override void SetStats(WeaponStats weaponStats)
		{
			base.SetStats(weaponStats);
			blackHoleCenter.SetParentWeapon(ParentWeapon);
			blackHoleCenter.SetStats(weaponStats);
			blackHoleGravitation.SetStats(weaponStats);
		}

		private void Update()
		{
			TickLifeTime();
		}
	}
}