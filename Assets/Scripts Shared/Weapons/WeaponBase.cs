using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data.Elements;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using DefaultNamespace.Data.Game;
using DefaultNamespace.Data.Weapons;
using Interfaces;
using Managers;
using NaughtyAttributes;
using Objects;
using Objects.Abilities;
using Objects.Items;
using Objects.Players.Scripts;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Weapons
{
	public abstract class WeaponBase : MonoBehaviour, IPlayerItem, IWeapon
	{
		[SerializeField] public bool useNetworkPool;
		[SerializeField][ShowAssetPreview] public GameObject spawnPrefab;
		[SerializeField][ShowAssetPreview] public GameObject spawnSubPrefab;
		[SerializeField] public WeaponEnum WeaponId;
		[SerializeField] public AttackType attackType;
		[SerializeField] public string Name;
		[SerializeField][TextArea] public string Description;
		[SerializeField] public float chanceToAppear;
		[SerializeField] public Sprite Icon;
		[SerializeField] public bool unlockOnAchievement;
		[ShowIf("unlockOnAchievement")]
		[SerializeField] public AchievementEnum requiredAchievement;
		[SerializeField] public Element element;
		[SerializeField] public WeaponStats weaponStats;
		[SerializeField] List<UpgradeData> availableUpgrades;
		protected PlayerStatsComponent _playerStatsComponent;
		protected float _timer;
		private Transform _transformCache;
		
		
		public string NameField => Name;
		public string DescriptionField => Description;
		public Sprite IconField => Icon;
		public int LevelField { get; private set; } = 1;
		public Element ElementField => element;
		public AttackType AttackTypeField => attackType;
		public IWeaponStatsStrategy WeaponStatsStrategy { get; set; }

		[HideInInspector] public bool isSkill;

		public bool IsItem => false;
		public AchievementEnum? RequiredAchievementField => unlockOnAchievement ? requiredAchievement : null;

		public string GetDescription(int rarity)
		{
			return weaponStats.GetDescription(Description, rarity);
		}

		public AttackType GetAttackType()
		{
			return attackType;
		}

		protected virtual void SetWeaponStatsStrategy()
		{
			WeaponStatsStrategy = new WeaponStatsStrategyBase(weaponStats, ElementField);
		}

		public virtual void Awake()
		{
			_playerStatsComponent = GetComponentInParent<PlayerStatsComponent>();
			weaponStats.AssignPlayerStatsComponent(_playerStatsComponent);
			SetWeaponStatsStrategy();

			_transformCache = transform;
			_timer = WeaponStatsStrategy.GetTotalCooldown();
			InitPool();
		}

		public void ActivateWeapon()
		{
			StartCoroutine(AttackProcess());
		}

		protected virtual void InitPool()
		{
			return;
		}

		public List<UpgradeData> GetAvailableUpgrades()
		{
			return availableUpgrades;
		}

		public void ApplyRarity(int rarity)
		{
			weaponStats.ApplyRarity(rarity);
		}

		public void Upgrade(UpgradeData upgradeData, int rarity)
		{
			LevelField++;
			availableUpgrades.Remove(upgradeData);
			weaponStats.Sum(upgradeData.WeaponStats, rarity);
			OnLevelUp();
		}

		
		public virtual void Update()
		{
			_timer -= Time.deltaTime;
			if (_timer >= 0f || PauseManager.instance.IsPauseStateSet(GamePauseStates.PauseWeaponAttacks)) return;

			_timer = WeaponStatsStrategy.GetTotalCooldown();
			if (GameManager.instance.playerStatsComponent.IsDead()) return;
			CustomUpdate();
			StartCoroutine(AttackProcess());
		}

		protected virtual IEnumerator AttackProcess()
		{
			OnAttackStart();
			for (var i = 0; i < GetAttackCount(); i++)
			{
				Attack();
				yield return new WaitForSeconds(WeaponStatsStrategy.GetDuplicateSpawnDelay());
			}
			OnAttackEnd();
		} 

		protected virtual int GetAttackCount()
		{
			return WeaponStatsStrategy.GetAttackCount();
		}
		
		protected float GetRotationByAttackCount()
		{
			return 360.0f / GetAttackCount();
		}
		
		public abstract void Attack();

		protected virtual void OnLevelUp() {}

		protected virtual void OnAttackStart() {}
		
		protected virtual void OnAttackEnd() {}
		
		protected virtual void CustomUpdate() {}

		public void ReduceCooldown(float reductionPercentage)
		{
			_timer *= 1 - reductionPercentage;
		}
		
		public virtual bool IsUnlocked(SaveFile saveFile)
		{
			return !unlockOnAchievement || saveFile.IsAchievementUnlocked(requiredAchievement);
		}

		public bool ReliesOnAchievement(AchievementEnum achievement)
		{
			return unlockOnAchievement && achievement == requiredAchievement;
		}

		public virtual void OnEnemyKilled()
		{
		}

		[Obsolete("Use override with pool id instead")]
		public virtual void SetupProjectile(NetworkProjectile networkProjectile)
		{
			
		}

		public virtual void SetupProjectile(NetworkProjectile networkProjectile, WeaponPoolEnum weaponPoolId)
		{
			if (weaponPoolId == WeaponPoolEnum.Main)
				SetupProjectile(networkProjectile);
		}

		public virtual GameObject GetProjectile(WeaponPoolEnum weaponPoolEnum)
		{
			return weaponPoolEnum == WeaponPoolEnum.Main ? spawnPrefab : spawnSubPrefab;
		}

		private void OnValidate()
		{
			if (spawnPrefab != null)
			{
				var networkProjectile = spawnPrefab.GetComponent<NetworkProjectile>();
				Assert.IsNotNull(networkProjectile, $"Weapon {NameField}: spawn prefab \"{spawnPrefab.name}\" has no {nameof(NetworkProjectile)} component.");
				Assert.IsTrue(networkProjectile.DesignedPoolId == WeaponPoolEnum.Main, $"Weapon {NameField}: spawn prefab \"{spawnPrefab.name}\" must have weapon type set to MAIN");
			}
			if (spawnSubPrefab != null)
			{
				var networkProjectile = spawnSubPrefab.GetComponent<NetworkProjectile>();
				Assert.IsNotNull(networkProjectile, $"Weapon {NameField}: spawn prefab \"{spawnSubPrefab.name}\" has no {nameof(NetworkProjectile)} component.");
				Assert.IsTrue(networkProjectile.DesignedPoolId == WeaponPoolEnum.Sub, $"Weapon {NameField}: spawn prefab \"{spawnSubPrefab.name}\" must have weapon type set to SUB");
			}
		}

		public IWeaponStatsStrategy GetWeaponStrategy()
		{
			return WeaponStatsStrategy;
		}

		public Transform GetTransform()
		{
			return _transformCache;
		}

		public bool IsUseNetworkPool()
		{
			return useNetworkPool;
		}

		public int GetId()
		{
			return (int)WeaponId;
		}

		public Element GetElement()
		{
			return ElementField;
		}

		public GameObject GetProjectile()
		{
			throw new NotImplementedException();
		}
	}
}