using System.Linq;
using DefaultNamespace;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.SpaceExpansionBall
{
	public class SpaceBallWeapon : WeaponBase
	{
		[HideInInspector] public bool IsGallacticCollapse;
		[SerializeField] public GameObject ExplosionPrefab;
		
		public override void Attack()
		{
			var spaceBall = SpawnManager.instance.SpawnObject(transform.position, spawnPrefab);
			var projectileComponent = spaceBall.GetComponent<SpaceBallProjectile>();
			projectileComponent.SetStats(weaponStats);
			projectileComponent.SetParentWeapon(this);
        
			var enemy = FindObjectsOfType<Damageable>().OrderBy(x => Random.value).FirstOrDefault();
			if (enemy is null)
				return;

			var position = enemy.transform.position;
			projectileComponent.SetDirection(position.x, position.y, position.z);
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 9)
				IsGallacticCollapse = true;
		}
	}
}