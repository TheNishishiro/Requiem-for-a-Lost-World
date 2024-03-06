using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data.Elements;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Interfaces;
using NaughtyAttributes;
using Objects;
using Objects.Abilities;
using Objects.Items;
using Objects.Players.Scripts;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Weapons
{
	public abstract class WeaponBase : MonoBehaviour, IPlayerItem
	{
		[SerializeField] public bool useNetworkPool;
		[SerializeField] public GameObject spawnPrefab;
		[SerializeField] public WeaponEnum WeaponId;
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
		
		
		public string NameField => Name;
		public string DescriptionField => Description;
		public Sprite IconField => Icon;
		public int LevelField { get; private set; } = 1;
		public Element ElementField => element;
		public IWeaponStatsStrategy WeaponStatsStrategy { get; set; }

		[HideInInspector] public bool isSkill;
		
		public ICollection<StatsDisplayData> GetStatsData()
		{
			return weaponStats.GetStatsDisplayData();
		}

		public string GetDescription(int rarity)
		{
			return weaponStats.GetDescription(Description, rarity);
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
			
			_timer = WeaponStatsStrategy.GetTotalCooldown();
			InitPool();
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
			if (_timer >= 0f) return;

			_timer = WeaponStatsStrategy.GetTotalCooldown();
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

		public virtual void SetupProjectile(NetworkProjectile networkProjectile)
		{
			
		}
	}
}