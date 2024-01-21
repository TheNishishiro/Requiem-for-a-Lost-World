using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events.Handlers;
using Events.Scripts;
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
		private Coroutine _moveSpeedBoostCoroutine;
		private Coroutine _attackBoostCoroutine;
		private Coroutine _statBoostCoroutine;
		private float _lastMoveSpeedIncrease;
		private float _lastAttackIncrease;
		
		public void Set(PlayerStats playerStats)
		{
			_playerStats ??= new PlayerStats();
			if (playerStats != null)
				_playerStats.Set(playerStats);

			if (GameData.GetPlayerCharacterId() == CharactersEnum.Arika_BoV && GameData.GetPlayerCharacterRank() == CharacterRank.E3)
				_playerStats.SkillCooldownReductionPercentage += 0.25f;
		}

		public void Apply(ItemStats itemStats, int rarity)
		{
			_playerStats.Sum(itemStats, rarity);
		}
		
		public void Add(PermUpgradeType permUpgradeType, float value)
		{
			_playerStats.Add(permUpgradeType, value);
		}

		public void ApplyPermanent(PermUpgrade permUpgrade, int upgradeLevel)
		{
			_playerStats.Add(permUpgrade.type, permUpgrade.increasePerLevel * upgradeLevel);
		}
		
		public void Add(StatEnum stat, float value)
		{
			_playerStats.Add(stat, value);
			StatChangedEvent.Invoke(stat, value);
		}

		public PlayerStats GetStats()
		{
			return _playerStats;
		}

		public IEnumerable<StatsDisplayData> GetStatsDisplayData()
		{
			return _playerStats.GetStatsList();
		}

		public void SetInvincible(bool isInvincible)
		{
			IsInvincible = isInvincible;
		}

		public void UseRevive()
		{
			_playerStats.Revives--;
		}

		public void SetHealth(float health)
		{
			_playerStats.Health = health;
		}

		public bool IsFullHealth()
		{
			return Math.Abs((_playerStats?.Health - _playerStats?.HealthMax).GetValueOrDefault()) < 0.01f;
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

		public void IncreaseMaxHealth(float amount)
		{
			_playerStats.HealthMax += amount;
		}

		public void IncreaseHealingReceived(float amount)
		{
			_playerStats.HealingIncreasePercentage += amount;
		}

		public void IncreaseExperienceGain(float amount)
		{
			_playerStats.ExperienceIncreasePercentage += amount;
		}

		public void IncreaseSkip(int amount)
		{
			_playerStats.Skips += amount;
		}

		public void IncreaseReroll(int amount)
		{
			_playerStats.Rerolls += amount;
		}

		public void IncreaseMovementSpeed(float amount)
		{
			_playerStats.MovementSpeed += amount;
		}

		public void IncreaseDamageOverTime(float amount)
		{
			_playerStats.DamageOverTime += amount;
		}

		public void IncreaseLuck(float amount)
		{
			_playerStats.Luck += amount;
		}

		public void IncreaseDamageTaken(float amount)
		{
			_playerStats.DamageTakenIncreasePercentage += amount;
		}
		
		public void IncreaseDodgeChance(float amount)
		{
			_playerStats.DodgeChance += amount;
		}
		
		public int GetTotalDamage(int baseDamage)
		{
			var damage = (baseDamage + PlayerStatsScaler.GetScaler().GetDamage());
			var critRate = PlayerStatsScaler.GetScaler().GetCritRate();
			var critDamage = PlayerStatsScaler.GetScaler().GetCritDamage();
			return  (int)Math.Ceiling((Random.value < critRate ? damage * critDamage : damage) * PlayerStatsScaler.GetScaler().GetDamageIncreasePercentage());
		}
		
		public void TakeDamage(float amount, bool isPreventDeath = false)
		{
			_playerStats.Health -= amount;

			switch (_playerStats.Health)
			{
				case < 0 when !isPreventDeath:
					_playerStats.Health = 0;
					break;
				case < 0:
					_playerStats.Health = 1;
					break;
				default:
				{
					if (_playerStats.Health > _playerStats.HealthMax)
						_playerStats.Health = _playerStats.HealthMax;
					break;
				}
			}
		}

		public void TemporaryStatBoost(StatEnum statEnum, float amount, float duration)
		{
			StartCoroutine(TempStatProcess(statEnum, amount, duration));
		}

		private IEnumerator TempStatProcess(StatEnum statEnum, float amount, float duration)
		{
			Add(statEnum, amount);
			yield return new WaitForSeconds(duration);
			Add(statEnum, -amount);
		}

		private readonly Dictionary<string, Coroutine> _tempUniqueBuffs = new ();
		public void TemporaryStatBoost(string id, StatEnum statEnum, float amount, float duration)
		{
			if (_tempUniqueBuffs.ContainsKey(id)) return;
			
			var coroutine = StartCoroutine(TempStatProcess(id, statEnum, amount, duration));
			_tempUniqueBuffs.Add(id, coroutine);
		}

		private IEnumerator TempStatProcess(string id, StatEnum statEnum, float amount, float duration)
		{
			Add(statEnum, amount);
			yield return new WaitForSeconds(duration);
			_tempUniqueBuffs[id] = null;
			_tempUniqueBuffs.Remove(id);
			Add(statEnum, -amount);
		}

		public void TemporaryMoveSpeedBoost(float increase, float duration)
		{
			if (_moveSpeedBoostCoroutine != null)
			{
				StopCoroutine(_moveSpeedBoostCoroutine);
				IncreaseMovementSpeed(-_lastMoveSpeedIncrease);
			}
			_lastMoveSpeedIncrease = increase;
			_moveSpeedBoostCoroutine = StartCoroutine(MoveSpeedBoostProcess(increase, duration));
		}

		private IEnumerator MoveSpeedBoostProcess(float amount, float duration)
		{
			IncreaseMovementSpeed(amount);
			yield return new WaitForSeconds(duration);
			IncreaseMovementSpeed(-amount);
			_moveSpeedBoostCoroutine = null;
		}

		public void TemporaryAttackBoost(float increase, float duration)
		{
			if (_attackBoostCoroutine != null)
			{
				StopCoroutine(_attackBoostCoroutine);
				IncreaseDamageIncreasePercentage(-_lastAttackIncrease);
			}
			_lastAttackIncrease = increase;
			_attackBoostCoroutine = StartCoroutine(AttackBoostProcess(increase, duration));
		}

		private IEnumerator AttackBoostProcess(float amount, float duration)
		{
			IncreaseDamageIncreasePercentage(amount);
			yield return new WaitForSeconds(duration);
			IncreaseDamageIncreasePercentage(-amount);
			_attackBoostCoroutine = null;
		}
	}
}