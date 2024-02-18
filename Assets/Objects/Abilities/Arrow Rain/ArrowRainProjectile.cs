using System;
using Data.Elements;
using Interfaces;
using Objects.Characters;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Pool;
using Weapons;

namespace Objects.Abilities.Arrow_Rain
{
	public class ArrowRainProjectile : PoolableProjectile<ArrowRainProjectile>
	{
		private ArrowRainWeapon ArrowRainWeapon => (ArrowRainWeapon) ParentWeapon;
		
		private void OnTriggerEnter(Collider other)
		{
			SimpleDamage(other, false, false, out var target);

			if (target == null)
				return;
			
			if (GameData.IsCharacterWithRank(CharactersEnum.Summer, CharacterRank.E5))
				target.ReduceElementalDefence(Element.Physical, 0.005f);
			
			if (ArrowRainWeapon.HailOfArrows)
				target.ReduceElementalDefence(Element.Physical, 0.005f);
		}
	}
}