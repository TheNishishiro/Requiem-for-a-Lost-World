using System;
using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Managers.StageEvents;
using Objects.Abilities.Laser_Gun;
using Objects.Characters;
using Objects.Stage;
using Unity.VisualScripting;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Back_Hole
{
	public class BlackHoleWeapon : PoolableWeapon<BlackHoleProjectile>
	{
		private StageTime _stageTime;
		private bool _isBelow5Minutes => Utilities.GetTimeSpan(_stageTime.time).Minutes <= 5;
		
		public override void Awake()
		{
			_stageTime = FindObjectOfType<StageTime>();
			base.Awake();
		}

		protected override bool ProjectileSpawn(BlackHoleProjectile projectile)
		{
			var spawnArea = 4.0f;
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Arika_BoV)
			{
				spawnArea = _isBelow5Minutes ? 1.5f : spawnArea;
			}

			var blackHolePosition = Utilities.GetRandomInAreaFreezeParameter(transform.position, spawnArea, isFreezeY:true);
			projectile.transform.position = Utilities.GetPointOnColliderSurface(blackHolePosition, transform);
			projectile.SetStats(weaponStats);
			return true;
		}

		protected override void OnAttackEnd()
		{
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Arika_BoV && _isBelow5Minutes)
				_timer /= 3;
			base.OnAttackEnd();
		}
	}
}