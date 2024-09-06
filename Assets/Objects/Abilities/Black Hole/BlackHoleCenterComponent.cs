using System;
using Interfaces;
using Objects.Characters;
using Objects.Stage;
using Unity.Netcode;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Back_Hole
{
	public class BlackHoleCenterComponent : ProjectileBase
	{
		[SerializeField] private NetworkProjectile networkProjectile;
		private float _weaknessIncrease;

		private void Start()
		{
			_weaknessIncrease = GameData.IsCharacterWithRank(CharactersEnum.Arika_BoV, CharacterRank.E3) ? 0.5f : 0;
		}

		private void OnTriggerStay(Collider other)
		{
			if (!networkProjectile.IsOwner) return;
			
			DamageArea(other, out var damageable);
			damageable?.SetVulnerable((WeaponEnum)ParentWeapon.GetId(), 1f, WeaponStatsStrategy.GetWeakness() + _weaknessIncrease);
		}
	}
}