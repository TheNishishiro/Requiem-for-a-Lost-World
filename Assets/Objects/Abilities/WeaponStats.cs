using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using DefaultNamespace;
using DefaultNamespace.Extensions;
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
		public float Damage;
		public float Cooldown;
		public float CooldownReduction;
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
		public float HealPerHit;
		public float DamageOverTime;
		public float DamageOverTimeDuration;
		public float DamageOverTimeFrequency;
		public float LifeSteal;
		public float FollowUpDamageIncrease;

		public void AssignPlayerStatsComponent(PlayerStatsComponent playerStatsComponent)
		{
			_playerStatsComponent = playerStatsComponent;
			_isInitialized = _playerStatsComponent != null; 
		}

		public void ApplyRarity(int rarity)
         {
	         var rarityFactor = GetRarityFactor(rarity);
         
             Damage *= rarityFactor;
             Cooldown *= 2 - rarityFactor;
             CooldownReduction *= rarityFactor;
             Scale *= rarityFactor;
             Speed *= rarityFactor;
             TimeToLive *= rarityFactor;
             DamageCooldown *= rarityFactor;
             DuplicateSpawnDelay *= rarityFactor;
             DetectionRange *= rarityFactor;
             CritRate *= rarityFactor;
             CritDamage *= rarityFactor;
             Weakness *= rarityFactor;
             DamageIncreasePercentage *= rarityFactor;
             HealPerHit *= rarityFactor;
             DamageOverTime *= rarityFactor;
             DamageOverTimeDuration *= rarityFactor;
             DamageOverTimeFrequency *= 2 - rarityFactor;
             LifeSteal *= rarityFactor;
             FollowUpDamageIncrease *= rarityFactor;
         }
		
		public void Sum(WeaponStats weaponStats, int rarity)
        {
	        var rarityFactor = GetRarityFactor(rarity);
              
            AttackCount += weaponStats.AttackCount;
            Damage += weaponStats.Damage * rarityFactor;
            Cooldown += weaponStats.Cooldown * rarityFactor;
            CooldownReduction += weaponStats.CooldownReduction * rarityFactor;
            Scale += weaponStats.Scale * rarityFactor;
            Speed += weaponStats.Speed * rarityFactor;
            TimeToLive += weaponStats.TimeToLive * rarityFactor;
            PassThroughCount += weaponStats.PassThroughCount; 
            DamageCooldown += weaponStats.DamageCooldown * rarityFactor;
            DuplicateSpawnDelay += weaponStats.DuplicateSpawnDelay * rarityFactor;
            DetectionRange += weaponStats.DetectionRange * rarityFactor;
            CritRate += weaponStats.CritRate * rarityFactor;
            CritDamage += weaponStats.CritDamage * rarityFactor;
            Weakness += weaponStats.Weakness * rarityFactor;
            DamageIncreasePercentage += weaponStats.DamageIncreasePercentage * rarityFactor;      
            HealPerHit += weaponStats.HealPerHit * rarityFactor;      
            DamageOverTime += weaponStats.DamageOverTime * rarityFactor;      
            DamageOverTimeDuration += weaponStats.DamageOverTimeDuration * rarityFactor;      
            DamageOverTimeFrequency += weaponStats.DamageOverTimeFrequency * (2 - rarityFactor);    
            LifeSteal += weaponStats.LifeSteal * rarityFactor;        
            FollowUpDamageIncrease += weaponStats.FollowUpDamageIncrease * rarityFactor;        
        }

		public string GetDescription(string description, int rarity)
		{
			var rarityFactor = GetRarityFactor(rarity);

			return description
				.Replace("{AttackCount}", Utilities.StatToString(AttackCount))
				.Replace("{Damage}", Utilities.StatToString(Damage, rarityFactor))
				.Replace("{Cooldown}", Utilities.StatToString(Cooldown, rarityFactor))
				.Replace("{CooldownReduction}", Utilities.StatToString(CooldownReduction, rarityFactor, true))
				.Replace("{Scale}", Utilities.StatToString(Scale, rarityFactor, true))
				.Replace("{Speed}", Utilities.StatToString(Speed, rarityFactor))
				.Replace("{TimeToLive}", Utilities.StatToString(TimeToLive, rarityFactor))
				.Replace("{PassThroughCount}", Utilities.StatToString(PassThroughCount))
				.Replace("{DamageCooldown}", Utilities.StatToString(DamageCooldown, rarityFactor))
				.Replace("{DuplicateSpawnDelay}", Utilities.StatToString(DuplicateSpawnDelay, rarityFactor))
				.Replace("{DetectionRange}", Utilities.StatToString(DetectionRange, rarityFactor))
				.Replace("{CritRate}", Utilities.StatToString(CritRate, rarityFactor, true))
				.Replace("{CritDamage}", Utilities.StatToString(CritDamage, rarityFactor, true))
				.Replace("{Weakness}", Utilities.StatToString(Weakness, rarityFactor, true))
				.Replace("{DamageIncreasePercentage}", Utilities.StatToString(DamageIncreasePercentage, rarityFactor, true))
				.Replace("{HealPerHit}", Utilities.StatToString(HealPerHit, rarityFactor))
				.Replace("{LifeSteal}", Utilities.StatToString(LifeSteal, rarityFactor, true))
				.Replace("{DamageOverTime}", Utilities.StatToString(DamageOverTime, rarityFactor))
				.Replace("{DamageOverTimeDuration}", Utilities.StatToString(DamageOverTimeDuration, rarityFactor))
				.Replace("{DamageOverTimeFrequency}", Utilities.StatToString(DamageOverTimeFrequency, rarityFactor, false, true))
				.Replace("{FollowUpDamageIncrease}", Utilities.StatToString(FollowUpDamageIncrease, rarityFactor, true))
				;
		}
		
		private float GetRarityFactor(float rarity)
		{
			const float percentIncreasePerRarity = 0.025f;
			return 1.0f + ((rarity - 1.0f) * percentIncreasePerRarity);
		}
	}
}