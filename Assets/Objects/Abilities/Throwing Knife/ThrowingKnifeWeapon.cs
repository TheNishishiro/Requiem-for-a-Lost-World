using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Abilities.Magic_Ball;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Throwing_Knife
{
	public class ThrowingKnifeWeapon : PoolableWeapon<ThrowingKnifeProjectile>
	{
		protected override bool ProjectileSpawn(ThrowingKnifeProjectile projectile)
		{
			projectile.transform.position = Utilities.GetRandomInAreaFreezeParameter(transform.position, 0.2f, isFreezeZ: true);
			projectile.SetDirection(transform.forward);
			projectile.SetParentWeapon(this);
			return true;
		}
	}
}