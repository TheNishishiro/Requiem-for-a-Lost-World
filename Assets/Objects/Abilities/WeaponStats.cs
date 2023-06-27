using System;
using Objects.Players.Scripts;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace Objects.Abilities
{
	[Serializable]
	public class WeaponStats
	{
		private PlayerStatsComponent _playerStatsComponent;
		private bool _isInitialized;
		public int Damage;
		public float Cooldown;
		public float Scale;
		public float Speed;
		public float TimeToLive;
		public int AttackCount;
		public int PassThroughCount;
		public float DamageCooldown;
		public float DuplicateSpawnDelay = 0.2f;
		public float DetectionRange;
		public float CritRate;
		public float CritDamage;
		public float Weakness;
		public float DamageIncreasePercentage;

		public void AssignPlayerStatsComponent(PlayerStatsComponent playerStatsComponent)
		{
			_playerStatsComponent = playerStatsComponent;
			_isInitialized = _playerStatsComponent != null; 
		}
		
		public void Sum(WeaponStats weaponStats)
		{
			Damage += weaponStats.Damage;
			Cooldown += weaponStats.Cooldown;
			Scale += weaponStats.Scale;
			Speed += weaponStats.Speed;
			TimeToLive += weaponStats.TimeToLive;
			AttackCount += weaponStats.AttackCount;
			PassThroughCount += weaponStats.PassThroughCount;
			DamageCooldown += weaponStats.DamageCooldown;
			DuplicateSpawnDelay += weaponStats.DuplicateSpawnDelay;
			CritRate += weaponStats.CritRate;
			CritDamage += weaponStats.CritDamage;
			Weakness += weaponStats.Weakness;
			DamageIncreasePercentage += weaponStats.DamageIncreasePercentage;
		}

		public float GetCooldown()
		{
			if (!_isInitialized) return Cooldown;

			var calculatedCooldown = (Cooldown - _playerStatsComponent.GetCooldownReduction()) * _playerStatsComponent.GetCooldownReductionPercentage();
			return calculatedCooldown <= 0 ? 0.01f : calculatedCooldown;
		}
		
		public int GetAttackCount()
		{
			if (!_isInitialized) return AttackCount;
			return AttackCount + _playerStatsComponent.GetAttackCount();
		}
		

		public int GetDamage()
		{
			if (!_isInitialized) 
				return (int) ((Random.value < CritRate ? Damage * CritDamage : Damage) * (1 + DamageIncreasePercentage));

			var damage = (Damage + _playerStatsComponent.GetDamage());
			var critRate = CritRate + _playerStatsComponent.GetCritRate();
			var critDamage = CritDamage + _playerStatsComponent.GetCritDamage();
			return  (int)Math.Ceiling((Random.value < critRate ? damage * critDamage : damage) * (_playerStatsComponent.GetDamageIncreasePercentage() + DamageIncreasePercentage));
		}
		
		public float GetScale()
		{
			if (!_isInitialized) return Scale;
			return Scale + _playerStatsComponent.GetScale();
		}
		
		public float GetSpeed()
		{
			if (!_isInitialized) return Speed;
			return Speed + _playerStatsComponent.GetProjectileSpeed();
		}
		
		public float GetTimeToLive()
		{
			if (!_isInitialized) return TimeToLive;
			return (TimeToLive + _playerStatsComponent.GetProjectileLifeTime()) * _playerStatsComponent.GetProjectileLifeTimeIncreasePercentage();
		}
		
		public float GetDetectionRange()
		{
			if (!_isInitialized) return DetectionRange;
			return DetectionRange + _playerStatsComponent.GetProjectileDetectionRange();
		}

		public int GetPassThroughCount()
		{
			if (!_isInitialized) return PassThroughCount;
			return PassThroughCount + _playerStatsComponent.GetProjectilePassThroughCount();
		}
		
		public float GetWeakness()
		{
			return Weakness;
		}
	}
}