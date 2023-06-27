using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Ice_Wave
{
	public class IceWaveWeapon : WeaponBase
	{
		[SerializeField] private int waveCount = 3;
		[SerializeField] private float scaleChangePerWave = 0.5f;
		private float rotateOffset = 0;
		private float currentWaveIndex = 0;
		public bool IsBlockingEnemies { get; private set; }
		private Vector3 startPosition;
		
		public override void Attack()
		{
			var currentScaleModifier = weaponStats.GetScale() * (scaleChangePerWave * currentWaveIndex);
			var newPosition = new Vector3(startPosition.x + (currentWaveIndex/3) * currentScaleModifier, 0, startPosition.z);
			
			var pointOnSurface = Utilities.GetPointOnColliderSurface(newPosition, transform);
			var iceCrystal = SpawnManager.instance.SpawnObject(pointOnSurface, spawnPrefab);
			iceCrystal.transform.RotateAround(startPosition, Vector3.up, rotateOffset);
			
			var projectileComponent = iceCrystal.GetComponent<IceWaveProjectile>();
			projectileComponent.SetParentWeapon(this);
			projectileComponent.SetStats(weaponStats);
			
			iceCrystal.transform.localScale = new Vector3(currentScaleModifier,currentScaleModifier,currentScaleModifier);
		}
		
		protected override IEnumerator AttackProcess()
		{
			var rotationStep = GetRotationByAttackCount();
			rotateOffset = 0;
			startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

			for (currentWaveIndex = 1; currentWaveIndex <= waveCount; currentWaveIndex++)
			{
				for (var i = 0; i < weaponStats.GetAttackCount(); i++)
				{
					Attack();
					rotateOffset += rotationStep;
				}
				
				yield return new WaitForSeconds(0.25f);
			}
		}

		protected override void OnLevelUp()
		{
			if (LevelField == 10)
				IsBlockingEnemies = true;
		}

		public override bool IsUnlocked(SaveFile saveFile)
		{
			return saveFile.IsAchievementUnlocked(AchievementEnum.Survive15MinutesWithOana);
		}
	}
}