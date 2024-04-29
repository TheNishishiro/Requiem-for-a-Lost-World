using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Objects.Players.PermUpgrades
{
    [CreateAssetMenu]
    public class StatsUiDataContainer : ScriptableObject
    {
        public List<StatsUiData> statsUiData;

        public Sprite GetIcon(StatEnum statId)
        {
            return statsUiData.FirstOrDefault(x => x.StatId == statId)?.SvgIcon;
        }
    }
}