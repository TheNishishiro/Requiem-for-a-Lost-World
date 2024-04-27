using System;
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

        private (Rarity, float) GetRandomRuneValue()
        {
            float minValue;
            float maxValue;
            var rarity = (Rarity)Random.Range((int)Rarity.None, (int)Rarity.Mythic + 1);
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

            var value = Random.Range(minValue, maxValue);
            // limit to one decimal place
            value = Mathf.Round(value * 10f) / 10f;
            
            return (rarity, value);
        }
    }
}