using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Abilities;
using Objects.Characters;
using Objects.Items;
using Objects.Players.PermUpgrades;
using Objects.Stage;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects.Players.Scripts
{
	public class PlayerStatsComponent : MonoBehaviour
	{
		private PlayerStats _playerStats;
		public bool IsInvincible;
		
		public void Set(PlayerStats playerStats)
		{
			_playerStats ??= new PlayerStats();
			if (playerStats != null)
				_playerStats.Set(playerStats);

			if (GameData.GetPlayerCharacterId() == CharactersEnum.Arika_BoV && GameData.GetPlayerCharacterRank() == CharacterRank.SS)
				_playerStats.SkillCooldownReductionPercentage += 0.25f;
		}

		public void SetInvincible(bool isInvincible)
		{
			IsInvincible = isInvincible;
		}

		public void UseRevive()
		{
			_playerStats.Revives--;
		}

		public void Apply(ItemStats itemStats, int rarity)
		{
			_playerStats.Sum(itemStats, rarity);
		}

		public void ApplyPermanent(PermUpgrade permUpgrade, int upgradeLevel)
		{
			_playerStats.Add(permUpgrade.type, permUpgrade.increasePerLevel * upgradeLevel);
		}

		public void SetHealth(float health)
		{
			_playerStats.Health = health;
		}

		public bool IsFullHealth()
		{
			return _playerStats?.Health - _playerStats?.HealthMax < 0.01f;
		}

		public bool IsDead()
		{
			return _playerStats?.Health <= 0 && !IsInvincible;
		}

		public void ApplyRegeneration()
		{
			TakeDamage(-_playerStats.HealthRegen);
		}

		public void IncreaseSpeed(float value)
		{
			_playerStats.Speed += value;
		}

		public void IncreaseCritRate(float critRateIncrease)
		{
			_playerStats.CritRate += critRateIncrease;
		}

		public void IncreaseCooldownReductionPercentage(float value)
		{
			_playerStats.CooldownReductionPercentage += value;
		}

		public void IncreaseCritDamage(float critDamageIncrease)
		{
			_playerStats.CritDamage += critDamageIncrease;
		}

		public void IncreaseAttackCount(int amount)
		{
			_playerStats.AttackCount += amount;
		}

		public void IncreaseEnemyHealth(float percentage)
		{
			_playerStats.EnemyHealthIncreasePercentage += percentage;
		}

		public void IncreaseFlatDamage(int amount)
		{
			_playerStats.Damage += amount;
		}

		public void IncreaseProjectileSize(float percentage)
		{
			_playerStats.Scale += percentage;
		}

		public void IncreaseDamageIncreasePercentage(float damageIncreasePercentage)
		{
			_playerStats.DamagePercentageIncrease += damageIncreasePercentage;
		}

		public int GetTotalDamage(int baseDamage)
		{
			var damage = (baseDamage + GetDamage());
			var critRate = GetCritRate();
			var critDamage = GetCritDamage();
			return  (int)Math.Ceiling((Random.value < critRate ? damage * critDamage : damage) * GetDamageIncreasePercentage());
		}

		public float GetHealth()
		{
			return _playerStats?.Health ?? 0;
		}

		public float GetMaxHealth()
		{
			return _playerStats?.HealthMax ?? 0;
		}
		
		public float GetEnemyHealthIncrease()
		{
			return 1 + _playerStats?.EnemyHealthIncreasePercentage ?? 0;
		}
		
		public float GetEnemySpawnRateIncrease()
		{
			return 1 + _playerStats?.EnemySpawnRateIncreasePercentage ?? 0;
		}
		
		public float GetEnemyCountIncrease()
		{
			return 1 + _playerStats?.EnemyMaxCountIncreasePercentage ?? 0;
		}
		
		public float GetEnemySpeedIncrease()
		{
			return 1 + _playerStats?.EnemySpeedIncreasePercentage ?? 0;
		}
		
		public float GetExperienceIncrease()
		{
			return 1 + _playerStats?.ExperienceIncreasePercentage ?? 0;
		}

		public float GetMovementSpeed()
		{
			return _playerStats?.MovementSpeed ?? 0;
		}

		public int GetArmor()
		{
			return _playerStats?.Armor ?? 0;
		}

		public float GetSkillCooldownReductionPercentage()
		{
			return 1 - _playerStats?.SkillCooldownReductionPercentage ?? 0;
		}

		public void TakeDamage(float amount)
		{
			_playerStats.Health -= amount;
			
			if (_playerStats.Health < 0)
				_playerStats.Health = 0;
			else if (_playerStats.Health > _playerStats.HealthMax)
				_playerStats.Health = _playerStats.HealthMax;
		}

		public double GetMagnetSize()
		{
			return _playerStats?.MagnetSize ?? 0;
		}

		public float GetCooldownReductionPercentage()
		{
			return 1 - (_playerStats?.CooldownReductionPercentage ?? 0);
		}

		public float GetCooldownReduction()
		{
			return _playerStats?.CooldownReduction ?? 0;
		}

		public int GetAttackCount()
		{
			return _playerStats?.AttackCount ?? 0;
		}

		public float GetDamageIncreasePercentage()
		{
			return 1 + (_playerStats?.DamagePercentageIncrease ?? 0);
		}

		public float GetDamage()
		{
			return _playerStats?.Damage ?? 0;
		}

		public double GetCritRate()
		{
			return _playerStats?.CritRate ?? 0;
		}

		public double GetCritDamage()
		{
			return 1 + (_playerStats?.CritDamage ?? 0.5);
		}

		public float GetScale()
		{
			return _playerStats?.Scale ?? 0;
		}

		public float GetProjectileSpeed()
		{
			return _playerStats?.Speed ?? 0;
		}

		public float GetProjectileLifeTime()
		{
			return _playerStats?.TimeToLive ?? 0;
		}

		public float GetProjectileDetectionRange()
		{
			return _playerStats?.DetectionRange ?? 0;
		}

		public int GetProjectilePassThroughCount()
		{
			return _playerStats?.PassThroughCount ?? 0;
		}

		public float GetItemRewardIncrease()
		{
			return 1 + (_playerStats?.ItemRewardIncrease ?? 0);
		}
		
		public int GetRevives()
		{
			return _playerStats?.Revives ?? 0;
		}

		public float GetProjectileLifeTimeIncreasePercentage()
		{
			return 1 + (_playerStats?.ProjectileLifeTimeIncreasePercentage ?? 0);
		}
	}
}