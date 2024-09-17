using System;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Players.PermUpgrades;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class RuneEquipmentEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelName;
        [SerializeField] private TextMeshProUGUI labelDescription;
        [SerializeField] private Material materialCommon;
        [SerializeField] private Material materialUncommon;
        [SerializeField] private Material materialRare;
        [SerializeField] private Material materialLegendary;
        [SerializeField] private Material materialMythic;
        [SerializeField] private SVGImage svgIcon;
        private RuneSaveData _runeSaveData;
        
        public void Setup(RuneSaveData runeSaveData)
        {
            _runeSaveData = runeSaveData;
            labelName.text = runeSaveData.runeName;
            labelDescription.text = RuneListManager.instance.GetDisplay(runeSaveData, true);
            labelName.fontSharedMaterial = runeSaveData.rarity switch
            {
                Rarity.None => materialCommon,
                Rarity.Common => materialUncommon,
                Rarity.Rare => materialRare,
                Rarity.Legendary => materialLegendary,
                Rarity.Mythic => materialMythic,
                _ => throw new ArgumentOutOfRangeException()
            };
            svgIcon.sprite = RuneListManager.instance.GetIcon(runeSaveData.statType);
        }

        public bool IsOfCategory(StatCategory statCategory)
        {
            return _runeSaveData.statType.GetStatType() == statCategory;
        }

        public void Discard()
        {
            RuneScreenManager.instance.Discard(_runeSaveData, this);
        }

        public void Equip()
        {
            RuneScreenManager.instance.Equip(_runeSaveData, this);
        }
    }
}