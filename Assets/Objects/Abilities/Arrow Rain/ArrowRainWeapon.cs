﻿using System;
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
	public class ArrowRainWeapon : WeaponBase
	{
		private Damageable _target;
		public bool HailOfArrows;
		private StageTime _stageTime;
		
		public override void Awake()
		{
			_stageTime = FindObjectOfType<StageTime>();
			base.Awake();
		}
		
		public override void Attack()
		{
			if (_target == null)
				OnAttackStart();

			var position = _target.transform.position;
			var arrowRain = SpawnManager.instance.SpawnObject(new Vector3(position.x, position.y + 2.5f, position.z), spawnPrefab);
			var projectileComponent = arrowRain.GetComponent<ArrowRainProjectile>();
			projectileComponent.SetStats(weaponStats);
			projectileComponent.SetParentWeapon(this);
		}

		protected override int GetAttackCount()
		{
			var baseAmount = base.GetAttackCount();
			
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Summer)
				baseAmount += (Utilities.GetTimeSpan(_stageTime.time).Minutes / 2);
			
			return baseAmount;
		}

		protected override void OnAttackStart()
		{
			_target = FindObjectsOfType<Damageable>()
				.OrderByDescending(x => x.Health)
				.ThenBy(x =>Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault();
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 9)
				HailOfArrows = true;
		}

		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Survive15MinutesWithSummer);
		}
	}
}