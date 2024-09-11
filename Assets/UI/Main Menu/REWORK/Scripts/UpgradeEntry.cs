using System;
using DefaultNamespace.Attributes;
using DefaultNamespace.Data;
using Objects.Characters;
using Objects.Players.PermUpgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class UpgradeEntry : MonoBehaviour
    {
        // TODO: Extract character fragments buying to it's own class
        [SerializeField] private TextMeshProUGUI labelTitle;
        [SerializeField] private TextMeshProUGUI labelDescription;
        [SerializeField] private TextMeshProUGUI labelRating;
        [SerializeField] private TextMeshProUGUI labelPrice;
        [SerializeField] private TextMeshProUGUI labelUpgrade;
        [SerializeField] private Button buttonSell;
        [SerializeField] private Image imageIcon;
        [SerializeField] private CharactersEnum characterId;
        [SerializeField] private bool useCharacterFragments;
        private PermUpgrade _permUpgrade;

        public void Setup(PermUpgrade permUpgrade)
        {
            _permUpgrade = permUpgrade;
            Refresh();
        }

        private void OnEnable()
        {
            if (characterId != CharactersEnum.Unknown)
                SetFragmentsOnTitle(characterId);
        }

        public void Refresh()
        {
            SaveFile.Instance.PermUpgradeSaveData.TryGetValue(_permUpgrade.type, out var savedUpgradeLevel);
            var upgradeMaxLevel = _permUpgrade.maxLevel;
            var cost = _permUpgrade.basePrice + _permUpgrade.costPerLevel * savedUpgradeLevel;
            
            labelTitle.text = $"{_permUpgrade.name} <size=60%><i><color=#E0E0E0>({savedUpgradeLevel}/{upgradeMaxLevel})</color></i></size>";
            labelDescription.text = _permUpgrade.description;
            imageIcon.sprite = _permUpgrade.icon;
            var isMaxLevel = savedUpgradeLevel >= upgradeMaxLevel;
            labelPrice.text = isMaxLevel ? "---" : cost.ToString("0") + " gold";
            labelUpgrade.text = isMaxLevel ? "Maxed" : "Upgrade";
            
            var canRefund = savedUpgradeLevel > 0;
            buttonSell.gameObject.SetActive(canRefund);
            
            var filledStars = new string('\u25c8', savedUpgradeLevel);
            var emptyStars = new string('\u25c7', upgradeMaxLevel - savedUpgradeLevel);
            labelRating.text = filledStars + emptyStars;
        }

        public void Upgrade()
        {
            SaveFile.Instance.PermUpgradeSaveData.TryGetValue(_permUpgrade.type, out var savedUpgradeLevel);
            var cost = _permUpgrade.basePrice + _permUpgrade.costPerLevel * savedUpgradeLevel;
            var isMaxLevel = savedUpgradeLevel >= _permUpgrade.maxLevel;
            
            if (SaveFile.Instance.Gold >= cost && !isMaxLevel)
            {
                SaveFile.Instance.Gold -= (ulong)cost;
                SaveFile.Instance.AddUpgradeLevel(_permUpgrade.type);
                Refresh();
            }
        }

        public void Sell()
        {
            SaveFile.Instance.PermUpgradeSaveData.TryGetValue(_permUpgrade.type, out var savedUpgradeLevel);
            var refundAmount = _permUpgrade.basePrice + _permUpgrade.costPerLevel * (savedUpgradeLevel - 1);
            if (savedUpgradeLevel > 0)
            {
                SaveFile.Instance.Gold += (ulong)refundAmount;
                SaveFile.Instance.RemoveUpgradeLevel(_permUpgrade.type);
                Refresh();
            }
        }

        public void Exchange(int exchangeId)
        {
            if (exchangeId == 0)
            {
                SaveFile.Instance.ExchangeGoldForGems();
            }
        }
        
        public void BuyFragments(int characterId)
        {
            var characterEnum = (CharactersEnum)characterId;
            SaveFile.Instance.ExchangeGemsForFragments(characterEnum);
            SetFragmentsOnTitle(characterEnum);
        }

        private void SetFragmentsOnTitle(CharactersEnum characterId)
        {
            if (useCharacterFragments)
            {
                var characterSaveData = SaveFile.Instance.GetCharacterSaveData(characterId);
                var characterRank = characterSaveData.GetRankEnum();
                var characterRankText = $"<color=yellow>{characterRank.GetStringValue()}</color>";
                var isMaxRank = characterRank == CharacterRank.E5;
                if (!characterSaveData.IsUnlocked)
                    characterRankText = "<color=black>Locked</color>";
                var fragmentsText = isMaxRank
                    ? ""
                    : $", {characterSaveData.Fragments}/50";
                
                labelTitle.text = labelTitle.text.Split("<size=")[0] + $"<size=75%>  ({characterRankText}{fragmentsText})</size>";
                
                labelPrice.text = isMaxRank ? "---" : "250 gold";
                labelUpgrade.text = isMaxRank ? "Maxed" : "Buy";
            }
        }
    }
}