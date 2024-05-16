using System;
using Objects.Stage;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Objects.Characters.Amelia_BoD
{
    public class AmelisanaScalingStrategy : CharacterScalingStrategyBase
    {
        private float _specialBuffer;
        private DateTime _lastBufferTime = DateTime.MinValue;
        
        public override void TakeDamage(float amount, bool isPreventDeath = false)
        {
            if (amount < 0)
            {
                if (GameData.IsCharacterRank(CharacterRank.E4))
                    base.TakeDamage(amount * 0.2f, isPreventDeath);
                
                amount *= GameData.IsCharacterRank(CharacterRank.E1) ? -3f : -1f;
                IncrementSpecial(amount);
                return;
            }

            base.TakeDamage(amount, isPreventDeath);
        }

        public override float GetDamageIncreasePercentage()
        {
            if (!GameData.IsCharacterRank(CharacterRank.E2))
                return base.GetDamageIncreasePercentage();
                   
            return base.GetDamageIncreasePercentage() + (1f - (GetSpecialValue() / GetSpecialMaxValue()));
        }

        public override void IncrementSpecial(float amount)
        {
            if (amount >= 0)
            {
                _specialBuffer += amount;
                if (_lastBufferTime.AddSeconds(5) > DateTime.Now) return;
                
                PlayerStats.SpecialValue += _specialBuffer;
                _specialBuffer = 0;
                _lastBufferTime = DateTime.Now;
                ClampSpecial();
            }
            else
            {
                PlayerStats.SpecialValue -= amount;
                ClampSpecial();
            }
        }

        public override float GetPassiveWeaponFlatDamageIncrease()
        {
            var bindConsumeAmount = GameData.IsCharacterRank(CharacterRank.E5) ? 0.04f : 0.085f;
            var canIncreaseDamage = base.GetSpecialValue() / GetSpecialMaxValue() >= bindConsumeAmount;
            if (!canIncreaseDamage)
                return base.GetPassiveWeaponFlatDamageIncrease();
            
            var amountToConsume = GetSpecialMaxValue() * bindConsumeAmount;
            var baseDamage = PlayerStats.SpecialValue;
            PlayerStats.SpecialValue -= amountToConsume;
            return baseDamage;
        }

        public override float GetSpecialMaxValue()
        {
            return PlayerStats.HealthMax * 5;
        }

        public override float GetSpecialValue()
        {
            return base.GetSpecialValue() + _specialBuffer;
        }
    }
}