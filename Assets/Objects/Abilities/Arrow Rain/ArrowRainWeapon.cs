using System;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Abilities.SpaceExpansionBall;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Objects.Abilities.Arrow_Rain
{
	public class ArrowRainWeapon : WeaponBase
	{
		private Damageable _target;
		public bool HailOfArrows;
		
		public override void Attack()
		{
			if (_target == null)
				return;

			var position = _target.transform.position;
			var arrowRain = SpawnManager.instance.SpawnObject(new Vector3(position.x, position.y + 2.5f, position.z), spawnPrefab);
			var projectileComponent = arrowRain.GetComponent<ArrowRainProjectile>();
			projectileComponent.SetStats(weaponStats);
			projectileComponent.SetParentWeapon(this);
		}

		protected override void OnAttackStart()
		{
			_target = FindObjectsOfType<Damageable>().OrderByDescending(x => x.Health).ThenBy(_ => Random.value).FirstOrDefault();
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