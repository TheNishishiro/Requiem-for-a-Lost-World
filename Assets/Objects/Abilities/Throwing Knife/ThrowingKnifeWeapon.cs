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
		public override void SetupProjectile(NetworkProjectile networkProjectile)
		{
			var position = Utilities.GetRandomInAreaFreezeParameter(transform.position, 0.2f, isFreezeZ: true);
			networkProjectile.Initialize(this, position);
			networkProjectile.GetProjectile<ThrowingKnifeProjectile>().SetDirection(transform.forward);
		}
	}
}