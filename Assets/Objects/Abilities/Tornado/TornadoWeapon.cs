using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Tornado
{
	public class TornadoWeapon : WeaponBase
	{
		public bool IsStaticDischarge { get; private set; }
		
		public override void Attack()
		{
			var pointOnSurface = Utilities.GetPointOnColliderSurface(new Vector3(transform.position.x, 0, transform.position.z), transform);
			var tornadoPosition = Utilities.GetRandomInAreaFreezeParameter(pointOnSurface, 4, isFreezeY:true);
			var tornado = SpawnManager.instance.SpawnObject(tornadoPosition, spawnPrefab);
			
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