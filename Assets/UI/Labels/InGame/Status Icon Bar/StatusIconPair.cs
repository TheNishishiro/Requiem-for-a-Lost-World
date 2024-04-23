using System;
using DefaultNamespace.Data.Statuses;
using UnityEngine;

namespace UI.Labels.InGame.Status_Icon_Bar
{
    [Serializable]
    public class StatusIconPair
    {
        public StatusEffectType statusEffectType;
        public Sprite icon;
        public bool isNegative;
    }
}