using System;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Managers.StageEvents;
using Objects.Abilities.SpaceExpansionBall;
using Objects.Characters;
using Objects.Stage;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.Arrow_Rain
{
	public class ArrowRainWeapon : PoolableWeapon<ArrowRainProjectile>
	{
		private Damageable _target;
		public bool HailOfArrows;
		private StageTime _stageTime;
		
		public override void Awake()
		{
			_stageTime = FindFirstObjectByType<StageTime>();
			base.Awake();
		}

		public override void SetupProjectile(NetworkProjectile networkProjectile)
		{
			if (_target == null)
			{
				OnAttackStart();
				if (_target == null)
				{
					networkProjectile.Despawn(WeaponId);
					return;
				}
			}
			
			var position = _target.transform.position;
			position = new Vector3(position.x, position.y + 2.5f, position.z);
			networkProjectile.Initialize(this, position);
		}

		protected override int GetAttackCount()
		{
			var baseAmount = base.GetAttackCount();
			
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Summer)
				baseAmount += (Utilities.GetTimeSpan(_stageTime.time.Value).Minutes / 2);
			
			return baseAmount;
		}

		protected override void OnAttackStart()
		{
			_target = EnemyManager.instance.GetActiveEnemies()
				.Select(x => x.GetDamagableComponent())
				.OrderByDescending(x => x.Health)
				.ThenBy(x =>Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault();
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 9)
				HailOfArrows = true;
		}
	}
}