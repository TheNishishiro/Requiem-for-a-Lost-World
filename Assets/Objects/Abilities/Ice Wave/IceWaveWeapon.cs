using System;
using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Characters;
using Objects.Players.PermUpgrades;
using Objects.Stage;
using UnityEngine;
using Weapons;

namespace Objects.Abilities.Ice_Wave
{
	public class IceWaveWeapon : PoolableWeapon<IceWaveProjectile>
	{
		[SerializeField] private int waveCount = 3;
		[SerializeField] private float scaleChangePerWave = 0.5f;
		private float _rotationStep = 0;
		private float _rotateOffset = 0;
		private float _currentWaveIndex = 0;
		public bool IsBlockingEnemies { get; private set; }
		private Vector3 _startPosition;

		private void Start()
		{
			if (GameData.GetPlayerCharacterId() == CharactersEnum.Oana_BoI &&
			    GameData.GetPlayerCharacterRank() == CharacterRank.E5)
				waveCount += 1;
		}

		protected override bool ProjectileSpawn(IceWaveProjectile projectile)
		{
			
			return true;
		}

		public override void SetupProjectile(NetworkProjectile networkProjectile)
		{
			var currentScaleModifier = WeaponStatsStrategy.GetScale() * (scaleChangePerWave * _currentWaveIndex);
			var newPosition = new Vector3(_startPosition.x + (_currentWaveIndex/2) * currentScaleModifier, 0, _startPosition.z);

			networkProjectile.Initialize(this, newPosition);
			networkProjectile.transform.RotateAround(_startPosition, Vector3.up, _rotateOffset);
			networkProjectile.transform.position = Utilities.GetPointOnColliderSurface(networkProjectile.transform.position, transform);
			networkProjectile.transform.localScale = new Vector3(currentScaleModifier,currentScaleModifier,currentScaleModifier);
			
			_rotateOffset += _rotationStep;
		}

		protected override IEnumerator AttackProcess()
		{
			OnAttackStart();
			_rotationStep = GetRotationByAttackCount();
			_rotateOffset = 0;
			_startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

			for (_currentWaveIndex = 1; _currentWaveIndex <= waveCount; _currentWaveIndex++)
			{
				for (var i = 0; i < GetAttackCount(); i++)
				{
					Attack();
				}
				
				yield return new WaitForSeconds(0.25f);
			}
			OnAttackEnd();
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

		public override void OnEnemyKilled()
		{
			base.OnEnemyKilled();
			var additional = GameManager.instance.playerComponent.GetLevel() * 0.01f;
			GameManager.instance.playerComponent.playerStatsComponent.TemporaryStatBoost("oana_kill_buff", StatEnum.DamagePercentageIncrease, additional, 1);
		}
	}
}