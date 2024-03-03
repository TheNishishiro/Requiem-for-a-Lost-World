using DefaultNamespace;
using Interfaces;
using Managers;
using Objects.Characters;
using Objects.Stage;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Katana
{
	public class KatanaWeapon : PoolableWeapon<KatanaProjectile>
	{
		protected override bool ProjectileSpawn(KatanaProjectile projectile)
		{
			var transform1 = transform;
			var slashPosition = transform1.position + transform1.forward/2;
			projectile.transform.position = slashPosition;
			projectile.SetParentWeapon(this);

			return true;
		}
		
		protected override int GetAttackCount()
		{
			var attackCount = base.GetAttackCount();
			if (GameData.IsCharacterWithRank(CharactersEnum.Nishi, CharacterRank.E4))
				attackCount += 2;
			
			return attackCount;
		}
	}
}