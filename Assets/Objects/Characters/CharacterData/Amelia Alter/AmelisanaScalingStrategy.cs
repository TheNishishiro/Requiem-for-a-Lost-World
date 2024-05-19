using System;
using Managers;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;

namespace Objects.Characters.Amelia_Alter
{
    public class AmelisanaScalingStrategy : CharacterScalingStrategyBase
    {
        private DateTime accumulationPauseTimer = DateTime.MinValue;
        
        public override void TakeDamage(float amount, bool isPreventDeath = false)
        {
            if (amount < 0)
            {
                if (GameData.IsCharacterRank(CharacterRank.E4))
                    base.TakeDamage(amount * 0.15f, isPreventDeath);
                
                if (DateTime.Now > accumulationPauseTimer)
                    IncrementSpecial(-amount);
                return;
            }

            var shieldPercentage = GameData.IsCharacterRank(CharacterRank.E1) ? 0.6f : 0.5f;
            var amountToShield = amount * shieldPercentage;

            var isShieldInsufficient = GetSpecialValue() < amountToShield;
            var damageToTake = amount * (1f-shieldPercentage) + (isShieldInsufficient ? amountToShield - GetSpecialValue() : 0);
            base.TakeDamage(damageToTake * shieldPercentage, isPreventDeath);
            PlayerStats.SpecialValue = isShieldInsufficient ? 0 : PlayerStats.SpecialValue - amountToShield;
            accumulationPauseTimer = DateTime.Now.AddSeconds(3);
        }

        public override float GetDamage()
        {
            if (!GameManager.IsCharacterState(PlayerCharacterState.Amelisana_Ultimate))
                return base.GetDamage();

            var conversionPercentage = GameData.IsCharacterRank(CharacterRank.E2) ? 0.5f : 0.25f;
            return base.GetDamage() + GetSpecialValue() * conversionPercentage;
        }

        public override float GetSpecialMaxValue()
        {
            return PlayerStats.HealthMax * 3;
        }
    }
}