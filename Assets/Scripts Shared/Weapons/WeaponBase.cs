using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data.Elements;
using DefaultNamespace.Data;
using Interfaces;
using Objects.Abilities;
using Objects.Items;
using Objects.Players.Scripts;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Weapons
{
	public abstract class WeaponBase : MonoBehaviour, IPlayerItem
	{
		[SerializeField] public GameObject spawnPrefab;
		[SerializeField] public string Name;
		[SerializeField][TextArea] public string Description;
		[SerializeField] public Sprite Icon;
		[SerializeField] public Element element;
		[SerializeField] protected WeaponStats weaponStats;
		[SerializeField] List<UpgradeData> availableUpgrades;
		private PlayerStatsComponent _playerStatsComponent;
		protected float _timer;
		
		public string NameField => Name;
		public string DescriptionField => Description;
		public Sprite IconField => Icon;
		public int LevelField { get; private set; } = 1;
		public Element ElementField => element;
		[HideInInspector] public bool isSkill;
		
		public ICollection<StatsDisplayData> GetStatsData()
		{
			return weaponStats.GetDescription();
		}

		public virtual void Awake()
		{
			_playerStatsComponent = GetComponentInParent<PlayerStatsComponent>();
			weaponStats.AssignPlayerStatsComponent(_playerStatsComponent);
			
			_timer = weaponStats.GetCooldown();
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

			_timer = weaponStats.GetCooldown();
			StartCoroutine(AttackProcess());
		}

		protected virtual IEnumerator AttackProcess()
		{
			OnAttackStart();
			for (var i = 0; i < GetAttackCount(); i++)
			{
				Attack();
				yield return new WaitForSeconds(weaponStats.DuplicateSpawnDelay);
			}
			OnAttackEnd();
		}

		protected virtual int GetAttackCount()
		{
			return weaponStats.GetAttackCount();
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
			_timer -= _timer * (1 - reductionPercentage);
		}
		
		public virtual bool IsUnlocked(SaveFile saveFile)
		{
			return true;
		}
	}
}