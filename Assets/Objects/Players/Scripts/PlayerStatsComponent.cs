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
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Objects.Players.Scripts
{
	public class PlayerStatsComponent : MonoBehaviour
	{
		public PlayerStats playerStats;
		public bool IsInvincible;
		private Coroutine _moveSpeedBoostCoroutine;
		private Coroutine _attackBoostCoroutine;
		private Coroutine _statBoostCoroutine;
		private float _lastMoveSpeedIncrease;
		private float _lastAttackIncrease;
		
		public void Set(PlayerStats playerStats)
		{
			this.playerStats ??= new PlayerStats();
			if (playerStats != null)
				this.playerStats.Set(playerStats);

			if (GameData.GetPlayerCharacterId() == CharactersEnum.Arika_BoV && GameData.GetPlayerCharacterRank() == CharacterRank.E3)
				this.playerStats.SkillCooldownReductionPercentage += 0.25f;
		}

		public void Apply(ItemStats itemStats, int rarity)
		{
			playerStats.Sum(itemStats, rarity);
		}
		
		public void Add(PermUpgradeType permUpgradeType, float value)
		{
			playerStats.Add(permUpgradeType, value);
		}

		public void ApplyPermanent(PermUpgrade permUpgrade, int upgradeLevel)
		{
			playerStats.Add(permUpgrade.type, permUpgrade.increasePerLevel * upgradeLevel);
		}
		
		public void Add(StatEnum stat, float value)
		{
			playerStats.Add(stat, value);
			StatChangedEvent.Invoke(stat, value);
		}

		public PlayerStats GetStats()
		{
			return playerStats;
		}

		public IEnumerable<StatsDisplayData> GetStatsDisplayData()
		{
			return playerStats.GetStatsList();
		}

		public void SetInvincible(bool isInvincible)
		{
			IsInvincible = isInvincible;
		}

		public void UseRevive()
		{
			playerStats.RevivesField--;
		}

		public void SetHealth(float health)
		{
			playerStats.Health = health;
		}

		public bool IsFullHealth()
		{
			return Math.Abs((playerStats?.Health - playerStats?.HealthMax).GetValueOrDefault()) < 0.01f;
		}

		public bool IsDead()
		{
			return playerStats?.Health <= 0 && !IsInvincible;
		}

		public void ApplyRegeneration()
		{
			TakeDamage(-playerStats.HealthRegen);
		}

		public void IncreaseSpeed(float value)
		{
			playerStats.Speed += value;
		}

		public void IncreaseCritRate(float critRateIncrease)
		{
			playerStats.CritRate += critRateIncrease;
		}

		public void IncreaseCooldownReductionPercentage(float value)
		{
			playerStats.CooldownReductionPercentage += value;
		}

		public void IncreaseCritDamage(float critDamageIncrease)
		{
			playerStats.CritDamage += critDamageIncrease;
		}

		public void IncreaseAttackCount(int amount)
		{
			playerStats.AttackCount += amount;
		}

		public void IncreaseEnemyHealth(float percentage)
		{
			playerStats.EnemyHealthIncreasePercentage += percentage;
		}

		public void IncreaseFlatDamage(int amount)
		{
			playerStats.Damage += amount;
		}

		public void IncreaseProjectileSize(float percentage)
		{
			playerStats.Scale += percentage;
		}

		public void IncreaseDamageIncreasePercentage(float damageIncreasePercentage)
		{
			playerStats.DamagePercentageIncrease += damageIncreasePercentage;
		}

		public void IncreaseMaxHealth(float amount)
		{
			playerStats.HealthMax += amount;
		}

		public void IncreaseHealingReceived(float amount)
		{
			playerStats.HealingIncreasePercentage += amount;
		}

		public void IncreaseExperienceGain(float amount)
		{
			playerStats.ExperienceIncreasePercentage += amount;
		}

		public void IncreaseSkip(int amount)
		{
			playerStats.Skips += amount;
		}

		public void IncreaseReroll(int amount)
		{
			playerStats.Rerolls += amount;
		}

		public void IncreaseMovementSpeed(float amount)
		{
			playerStats.MovementSpeed += amount;
		}

		public void IncreaseDamageOverTime(float amount)
		{
			playerStats.DamageOverTime += amount;
		}

		public void IncreaseLuck(float amount)
		{
			playerStats.Luck += amount;
		}

		public void IncreaseDamageTaken(float amount)
		{
			playerStats.DamageTakenIncreasePercentage += amount;
		}
		
		public void IncreaseDodgeChance(float amount)
		{
			playerStats.DodgeChance += amount;
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
			playerStats.Health -= amount;

			switch (playerStats.Health)
			{
				case < 0 when !isPreventDeath:
					playerStats.Health = 0;
					break;
				case < 0:
					playerStats.Health = 1;
					break;
				default:
				{
					if (playerStats.Health > playerStats.HealthMax)
						playerStats.Health = playerStats.HealthMax;
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