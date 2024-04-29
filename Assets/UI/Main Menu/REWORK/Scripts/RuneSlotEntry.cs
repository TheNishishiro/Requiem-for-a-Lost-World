using System;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Players.PermUpgrades;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class RuneSlotEntry : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private SVGImage statTypeImage;
        [SerializeField] private Material materialCommon;
        [SerializeField] private Material materialUncommon;
        [SerializeField] private Material materialRare;
        [SerializeField] private Material materialLegendary;
        [SerializeField] private Material materialMythic;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TextMeshProUGUI textStats;
        [SerializeField] private ParticleSystem particles;
        private RuneSaveData _runeSaveData;
        
        public void Setup(RuneSaveData runeSaveData, bool isInit)
        {
            _runeSaveData = runeSaveData;

            var value = RuneListManager.instance.GetScaledValue(runeSaveData);
            textStats.text = $"{runeSaveData.statType.GetShortName()} " + (runeSaveData.statType.IsPercent() ? $"{value*100:0.##}%" : $"{value:0.##}");
            textStats.fontSharedMaterial = runeSaveData.rarity switch
            {
                Rarity.None => materialCommon,
                Rarity.Common => materialUncommon,
                Rarity.Rare => materialRare,
                Rarity.Legendary => materialLegendary,
                Rarity.Mythic => materialMythic,
                _ => throw new ArgumentOutOfRangeException()
            };
            statTypeImage.sprite = RuneListManager.instance.GetIcon(runeSaveData.statType);
            container.SetActive(true);
            particles.gameObject.SetActive(!isInit);
        }

        public bool IsEmpty()
        {
            return _runeSaveData == null;
        }

        public void Clear()
        {
            _runeSaveData = null;
            container.SetActive(false);
            particles.gameObject.SetActive(false);
        }

        public void UnEquipRune()
        {
            RuneScreenManager.instance.UnEquipRune(this, _runeSaveData);
        }
    }
}