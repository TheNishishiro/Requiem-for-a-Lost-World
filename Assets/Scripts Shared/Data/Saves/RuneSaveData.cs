using System;
using DefaultNamespace.Data.Achievements;
using Objects.Players.PermUpgrades;

namespace DefaultNamespace.Data
{
    [Serializable]
    public class RuneSaveData
    {
        public StatEnum statType;
        public Rarity rarity;
        public string runeName;
        public float runeValue;

        public string GetName()
        {
            return $"{runeName} +{runeValue}";
        }

        public string GetShortStats()
        {
            return $"{runeValue}";
        }
    }
}