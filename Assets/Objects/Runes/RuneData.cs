using System;
using System.Linq;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Objects.Players.PermUpgrades;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects.Runes
{
    [CreateAssetMenu]
    public class RuneData : ScriptableObject
    {
        public string runeName;
        public StatEnum statType;
        public float commonMinValue;
        public float commonMaxValue;
        public float uncommonMaxValue;
        public float rareMaxValue;
        public float legendaryMaxValue;
        public float mythicMaxValue;

        public RuneSaveData ResolveRune()
        {
            var (rarity, runeValue) = GetRandomRuneValue();
            return new RuneSaveData { runeName = runeName, statType = statType, runeValue = runeValue, rarity = rarity};
        }

        public float GetScaledValue(RuneSaveData runeSaveData)
        {
            return GetScaledValue(runeSaveData.rarity, runeSaveData.runeValue);
        }

        public float GetScaledValue(Rarity rarity, float runeValue)
        {
            float minValue;
            float maxValue;
            
            switch (rarity)
            {
                case Rarity.None:
                    minValue = commonMinValue;
                    maxValue = commonMaxValue;
                    break;
                case Rarity.Common:
                    minValue = commonMaxValue;
                    maxValue = uncommonMaxValue;
                    break;
                case Rarity.Rare:
                    minValue = uncommonMaxValue;
                    maxValue = rareMaxValue;
                    break;
                case Rarity.Legendary:
                    minValue = rareMaxValue;
                    maxValue = legendaryMaxValue;
                    break;
                case Rarity.Mythic:
                    minValue = legendaryMaxValue;
                    maxValue = mythicMaxValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (minValue > maxValue)
            {
                (maxValue, minValue) = (minValue, maxValue);
            }

            if (statType.IsPercent())
                return Mathf.Round(Mathf.Lerp(minValue, maxValue, runeValue) * 1000f) / 1000f;
            if (statType.IsInteger())
                return (int)Mathf.Round(Mathf.Lerp(minValue, maxValue, runeValue));
            return Mathf.Round(Mathf.Lerp(minValue, maxValue, runeValue) * 100f) / 100f;
        }

        private (Rarity, float) GetRandomRuneValue()
        {
            var rarityValue = Random.value;
            var rarity = rarityValue switch
            {
                <= 0.4f => Rarity.None,
                <= 0.7f => Rarity.Common,
                <= 0.85f => Rarity.Rare,
                <= 0.95f => Rarity.Legendary,
                _ => Rarity.Mythic
            };

            var rangeValue = Random.Range(0f, 1f);
            return (rarity, rangeValue);
        }
    }
}