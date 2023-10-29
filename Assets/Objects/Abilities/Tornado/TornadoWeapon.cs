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
	public class TornadoWeapon : PoolableWeapon<TornadoProjectile>
	{
		private StageTime _stageTime;
		public bool IsStaticDischarge { get; private set; }
		
		public override void Awake()
		{
			_stageTime = FindFirstObjectByType<StageTime>();
			base.Awake();
		}

		protected override bool ProjectileSpawn(TornadoProjectile projectile)
		{
			var spawnRadius = GameData.GetPlayerCharacterId() == CharactersEnum.Natalie_BoW ? 
				Mathf.Lerp(1, 4, (float)Utilities.GetTimeSpan(_stageTime.time).TotalSeconds / 300.0f)  
				: 4;
			
			var tornadoPosition = Utilities.GetRandomInAreaFreezeParameter(transform.position, spawnRadius, isFreezeY:true);
			var pointOnSurface = Utilities.GetPointOnColliderSurface(tornadoPosition, transform);
			projectile.transform.position = pointOnSurface;
			projectile.gameObject.SetActive(true);
			projectile.SetStats(weaponStats);
			return true;
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