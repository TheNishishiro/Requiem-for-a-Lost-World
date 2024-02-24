using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data.Statuses;
using UnityEngine;

namespace UI.Labels.InGame.Status_Icon_Bar
{
    public class StatusIconContainer : MonoBehaviour
    {
        [SerializeField] private GameObject statusIconPrefab;
        [SerializeField] private StatusIconData statusIconData;
        private List<StatusIcon> statusIcons;

        private void Awake()
        {
            statusIcons = new List<StatusIcon>();
            for (var i = 0; i < 13; i++)
            {
                var statusIcon = Instantiate(statusIconPrefab, transform).GetComponent<StatusIcon>();
                statusIcon.gameObject.SetActive(false);
                statusIcons.Add(statusIcon);
            }
        }

        public void DisplayStatus(StatusEffectType statusEffect, int stackCount = 0)
        {
            var statusIconToDisplay = FindActiveOrInactiveIcon(statusEffect);

            if (statusIconToDisplay == null)
                return;

            statusIconToDisplay.Show(statusIconData.GetIcon(statusEffect), statusEffect, stackCount);
        }

        public void RemoveStatus(StatusEffectType statusEffect)
        {
            var activeIconForStatusEffect = statusIcons.Find(icon => icon.statusEffectType == statusEffect && icon.gameObject.activeSelf);
            if (activeIconForStatusEffect == null)
                return;
            
            activeIconForStatusEffect.Hide();
        }

        private StatusIcon FindActiveOrInactiveIcon(StatusEffectType statusEffect)
        {
            var activeIconForStatusEffect = statusIcons.Find(icon => icon.statusEffectType == statusEffect && icon.gameObject.activeSelf);
            if (activeIconForStatusEffect != null)
                return activeIconForStatusEffect;

            var anyInactiveIcon = statusIcons.Find(icon => !icon.gameObject.activeSelf);
            return anyInactiveIcon;
        }
    }
}