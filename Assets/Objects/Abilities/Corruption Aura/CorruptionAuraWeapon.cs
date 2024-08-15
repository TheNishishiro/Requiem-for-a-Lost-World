using System;
using Data.Elements;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using DefaultNamespace.Data.Weapons;
using Interfaces;
using Managers;
using Objects.Characters;
using Objects.Enemies;
using Objects.Stage;
using Unity.Netcode;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.Corruption_Aura
{
	public class CorruptionAuraWeapon : WeaponBase
	{
		private bool isProjectileSpawned;

		public override void SetupProjectile(NetworkProjectile networkProjectile)
		{
			var projectile = networkProjectile.GetProjectile<CorruptedAuraProjectile>();
			networkProjectile.Initialize(this, transform.position);
			projectile.FollowTransform(transform);
			isProjectileSpawned = true;
		}

		public override void Attack()
		{
			if (!isProjectileSpawned)
			{
				RpcManager.instance.FireProjectileRpc(WeaponId, transform.position, NetworkManager.Singleton.LocalClientId, WeaponPoolEnum.Main);
			}
		}
	}
}