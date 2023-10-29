using System;
using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Characters;
using Objects.Stage;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Ice_Wave
{
	public class IceWaveWeapon : PoolableWeapon<IceWaveProjectile>
	{
		[SerializeField] private int waveCount = 3;
		[SerializeField] private float scaleChangePerWave = 0.5f;
		private float rotateOffset = 0;
		private float currentWaveIndex = 0;
		public bool IsBlockingEnemies { get; private set; }
		private Vector3 startPosition;

		private void Start()
		{
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Oana_BoI &&
			    GameData.GetPlayerCharacterRank() == CharacterRank.E5)
				waveCount += 3;
		}

		protected override bool ProjectileSpawn(IceWaveProjectile projectile)
		{
			var currentScaleModifier = weaponStats.GetScale() * (scaleChangePerWave * currentWaveIndex);
			var newPosition = new Vector3(startPosition.x + (currentWaveIndex/2) * currentScaleModifier, 0, startPosition.z);

			projectile.transform.position = newPosition;
			projectile.transform.RotateAround(startPosition, Vector3.up, rotateOffset);
			projectile.transform.position = Utilities.GetPointOnColliderSurface(projectile.transform.position, transform);
			projectile.SetStats(weaponStats);
			
			projectile.transform.localScale = new Vector3(currentScaleModifier,currentScaleModifier,currentScaleModifier);
			return true;
		}

		protected override IEnumerator AttackProcess()
		{
			var rotationStep = GetRotationByAttackCount();
			rotateOffset = 0;
			startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

			for (currentWaveIndex = 1; currentWaveIndex <= waveCount; currentWaveIndex++)
			{
				for (var i = 0; i < GetAttackCount(); i++)
				{
					Attack();
					rotateOffset += rotationStep;
				}
				
				yield return new WaitForSeconds(0.25f);
			}
		}

		protected override int GetAttackCount()
		{
			var attacks = base.GetAttackCount();
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Oana_BoI &&
			    GameData.GetPlayerCharacterRank() >= CharacterRank.E1)
			{
				attacks += 2;
			}

			return attacks;
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