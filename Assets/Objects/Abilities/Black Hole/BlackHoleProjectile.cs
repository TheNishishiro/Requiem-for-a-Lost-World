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

		protected override void OnStateChanged(ProjectileState state)
		{
			if (state == ProjectileState.Flying)
			{
				blackHoleCenter.SetParentWeapon(ParentWeapon);
				blackHoleGravitation.SetParentWeapon(ParentWeapon);
			}
		}
	}
}