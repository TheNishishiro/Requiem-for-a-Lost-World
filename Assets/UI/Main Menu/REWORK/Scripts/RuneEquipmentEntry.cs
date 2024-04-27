using System;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Objects.Players.PermUpgrades;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class RuneEquipmentEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelName;
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
            var value = runeSaveData.statType.IsPercent() ? $"{runeSaveData.runeValue*100:N0}%" : $"{runeSaveData.runeValue}";
            labelName.text = $"{runeSaveData.runeName} +{value}";
            labelName.material = runeSaveData.rarity switch
            {
                Rarity.None => materialCommon,
                Rarity.Common => materialUncommon,
                Rarity.Rare => materialRare,
                Rarity.Legendary => materialLegendary,
                Rarity.Mythic => materialMythic,
                _ => throw new ArgumentOutOfRangeException()
            };
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