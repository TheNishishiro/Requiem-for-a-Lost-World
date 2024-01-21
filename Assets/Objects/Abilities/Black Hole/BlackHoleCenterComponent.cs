using System;
using Interfaces;
using Objects.Characters;
using Objects.Stage;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Back_Hole
{
	public class BlackHoleCenterComponent : ProjectileBase
	{
		private float _weaknessIncrease;

		private void Start()
		{
			_weaknessIncrease = GameData.IsCharacterWithRank(CharactersEnum.Arika_BoV, CharacterRank.E3) ? 0.5f : 0;
		}

		private void OnTriggerStay(Collider other)
		{
			DamageArea(other, out var damageable);
			damageable?.SetVulnerable(1f, WeaponStatsStrategy.GetWeakness() + _weaknessIncrease);
		}
	}
}