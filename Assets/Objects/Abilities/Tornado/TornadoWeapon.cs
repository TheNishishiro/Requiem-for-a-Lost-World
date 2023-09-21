using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Managers.StageEvents;
using Objects.Characters;
using Objects.Stage;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Tornado
{
	public class TornadoWeapon : WeaponBase
	{
		private StageTime _stageTime;
		public bool IsStaticDischarge { get; private set; }
		
		public override void Awake()
		{
			_stageTime = FindObjectOfType<StageTime>();
			base.Awake();
		}
		
		public override void Attack()
		{
			var spawnRadius = GameData.GetPlayerCharacterId() == CharactersEnum.Natalie_BoW ? 
				Mathf.Lerp(1, 4, (float)Utilities.GetTimeSpan(_stageTime.time).TotalSeconds / 300.0f)  
				: 4;
			
			var tornadoPosition = Utilities.GetRandomInAreaFreezeParameter(transform.position, spawnRadius, isFreezeY:true);
			var pointOnSurface = Utilities.GetPointOnColliderSurface(tornadoPosition, transform);
			var tornado = SpawnManager.instance.SpawnObject(pointOnSurface, spawnPrefab);
			
			var projectileComponent = tornado.GetComponent<TornadoProjectile>();
			projectileComponent.SetParentWeapon(this);
			projectileComponent.SetStats(weaponStats);
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 10)
				IsStaticDischarge = true;
		}

		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Survive15MinutesWithNatalie);
		}
	}
}