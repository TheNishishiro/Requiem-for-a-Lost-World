using System.Collections.Generic;
using DefaultNamespace.Data.Statuses;
using UnityEngine;

namespace UI.Labels.InGame.Status_Icon_Bar
{
    [CreateAssetMenu]
    public class StatusIconData : ScriptableObject
    {
        public List<StatusIconPair> statusIcons;
        
        public StatusIconPair GetStatusData(StatusEffectType statusEffect)
        {
            return statusIcons.Find(x => x.statusEffectType == statusEffect);
        }
    }
}