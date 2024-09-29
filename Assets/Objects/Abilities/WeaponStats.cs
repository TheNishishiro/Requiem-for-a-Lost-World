using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using DefaultNamespace;
using DefaultNamespace.Data.Locale;
using DefaultNamespace.Extensions;
using Objects.Players.Scripts;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using static DefaultNamespace.Utilities;

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
		public float ResPen;

		public void AssignPlayerStatsComponent(PlayerStatsComponent playerStatsComponent)
		{
			_playerStatsComponent = playerStatsComponent;
			_isInitialized = _playerStatsComponent != null; 
		}

		public void ApplyRarity(int rarity)
         {
	         var rarityFactor = GetUnlockRarityFactor(rarity);
         
             Damage *= rarityFactor;
             //Cooldown *= 2 - rarityFactor;
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
             ResPen *= rarityFactor;
         }
		
		public void Sum(WeaponStats weaponStats, int rarity)
        {
	        var rarityFactor = GetUpgradeRarityFactor(rarity);
              
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
            weaponStats.DamageOverTimeFrequency += AdjustForRarity(weaponStats.DamageOverTimeFrequency, rarityFactor);
            LifeSteal += weaponStats.LifeSteal * rarityFactor;        
            FollowUpDamageIncrease += weaponStats.FollowUpDamageIncrease * rarityFactor;        
            ResPen += weaponStats.ResPen * ResPen;        
        }

		public string GetDescription(string description, int rarity, bool isUnlock)
		{
			var rarityFactor = isUnlock ? GetUnlockRarityFactor(rarity) : GetUpgradeRarityFactor(rarity);

			return description.Translate()
				.Replace("{AttackCount}", StatToString(AttackCount))
				.Replace("{Damage}", StatToString(Damage, rarityFactor))
				.Replace("{Cooldown}", StatToString(Cooldown, rarityFactor))
				.Replace("{CooldownReduction}", StatToString(CooldownReduction, rarityFactor, true))
				.Replace("{Scale}", StatToString(Scale, rarityFactor, true))
				.Replace("{Speed}", StatToString(Speed, rarityFactor))
				.Replace("{TimeToLive}", StatToString(TimeToLive, rarityFactor))
				.Replace("{PassThroughCount}", StatToString(PassThroughCount))
				.Replace("{DamageCooldown}", StatToString(DamageCooldown, rarityFactor))
				.Replace("{DuplicateSpawnDelay}", StatToString(DuplicateSpawnDelay, rarityFactor))
				.Replace("{DetectionRange}", StatToString(DetectionRange, rarityFactor))
				.Replace("{CritRate}", StatToString(CritRate, rarityFactor, true))
				.Replace("{CritDamage}", StatToString(CritDamage, rarityFactor, true))
				.Replace("{Weakness}", StatToString(Weakness, rarityFactor, true))
				.Replace("{DamageIncreasePercentage}", StatToString(DamageIncreasePercentage, rarityFactor, true))
				.Replace("{HealPerHit}", StatToString(HealPerHit, rarityFactor))
				.Replace("{LifeSteal}", StatToString(LifeSteal, rarityFactor, true))
				.Replace("{DamageOverTime}", StatToString(DamageOverTime, rarityFactor))
				.Replace("{DamageOverTimeDuration}", StatToString(DamageOverTimeDuration, rarityFactor))
				.Replace("{DamageOverTimeFrequency}", StatToString(DamageOverTimeFrequency, rarityFactor, false, DamageOverTimeFrequency >= 0))
				.Replace("{FollowUpDamageIncrease}", StatToString(FollowUpDamageIncrease, rarityFactor, true))
				.Replace("{ResPen}", StatToString(ResPen, rarityFactor, true))
				;
		}
		
		private float GetUnlockRarityFactor(float rarity)
		{
			const float percentIncreasePerRarity = 0.05f;
			return 1.0f + ((rarity - 1.0f) * percentIncreasePerRarity);
		}
		
		private float GetUpgradeRarityFactor(float rarity)
		{
			const float percentIncreasePerRarity = 0.1f;
			return 1.0f + ((rarity - 1.0f) * percentIncreasePerRarity);
		}
	}
}