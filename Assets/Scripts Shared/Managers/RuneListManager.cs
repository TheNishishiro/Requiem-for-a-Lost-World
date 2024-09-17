using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Objects.Players.PermUpgrades;
using Objects.Runes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class RuneListManager : MonoBehaviour
    {
        public static RuneListManager instance;
        [SerializeField] private List<RuneData> runes;
        [SerializeField] private StatsUiDataContainer statsUiDataContainer;

        public void Start()
        {
            if (instance == null)
                instance = this;
        }

        public List<RuneData> GetRunes()
        {
            return runes;
        }

        public RuneData GetRandomRune()
        {
            return runes.OrderBy(x => Random.value).FirstOrDefault();
        }

        public float GetScaledValue(RuneSaveData runeSaveData)
        {
            var runeData = runes.FirstOrDefault(x => x.statType == runeSaveData.statType);
            return runeData.GetScaledValue(runeSaveData.rarity, runeSaveData.runeValue);
        }

        public string GetDisplay(RuneSaveData runeSaveData, bool useLongName)
        {
            var runeValue = GetScaledValue(runeSaveData);
            var sign = runeValue >= 0 ? '+' : '-';
            var value = runeSaveData.statType.IsPercent() ? $"{runeValue*100:0.##}%" : $"{runeValue:0.##}";
            var runeName = useLongName ? runeSaveData.statType.GetLongName() : runeSaveData.statType.GetShortName();
            return $"{runeName} {sign}{value}";
        }

        public Sprite GetIcon(StatEnum statEnum)
        {
            return statsUiDataContainer.GetIcon(statEnum);
        }
    }
}