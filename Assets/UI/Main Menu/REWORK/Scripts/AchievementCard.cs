using System;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Interfaces;
using Managers;
using Objects.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class AchievementCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Image iconUnlockable;
        [SerializeField] private Image progressBar;
        [SerializeField] private Image background;
        [SerializeField] private WeaponContainer weaponContainer;
        [SerializeField] private ItemContainer itemContainer;
        [SerializeField] private Material materialBlackAndWhite;
        [SerializeField] private Sprite iconDefault;
        [SerializeField] private TextMeshProUGUI textCompletionPercentage;
        [SerializeField] private GameObject completionPercentageContainer;
        private IPlayerItem _unlocksItem;
        private AchievementValueAttribute _achievementData;
        private AchievementEnum _achievementId;
        private AchievementState _filterState;
        private bool _isUnlocked;
        
        public void Setup(AchievementValueAttribute achievementValue, AchievementEnum achievementEnum)
        {
            _achievementData = achievementValue;
            _achievementId = achievementEnum;
            
            title.text = achievementValue.Title;
            _unlocksItem = weaponContainer.GetWeaponByAchievement(achievementEnum) ??
                           itemContainer.GetItemByAchievement(achievementEnum);
            iconUnlockable.sprite = _unlocksItem?.IconField;
            if (iconUnlockable.sprite == null)
                iconUnlockable.sprite = iconDefault;

            Refresh();
        }

        public void VisibleState(AchievementState state)
        {
            _filterState = state;
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            var filterUnlockState = _filterState == AchievementState.Completed;
            var isVisible = _isUnlocked == filterUnlockState || _filterState == AchievementState.All;
            gameObject.SetActive(isVisible);
        }

        public void OpenAchievementDisplay()
        {
            GetComponentInParent<AchievementScreenManager>().OpenAchievementDisplay(_achievementData, _unlocksItem);
        }

        public void Refresh()
        {
            var color = Utilities.RarityToColor(_achievementData.Rarity);
            SaveFile.Instance.AchievementSaveData.TryGetValue(_achievementId, out _isUnlocked);
            progressBar.color = color;
            if (!_isUnlocked)
                color = Color.grey;
            if (_unlocksItem == null)
                iconUnlockable.color = color;
            background.color = color;
            iconUnlockable.material = _isUnlocked ? null : materialBlackAndWhite;
            completionPercentageContainer.SetActive(!_isUnlocked);

            var requirementValue = _achievementData.Requirement.Value;
            var saveFile = SaveFile.Instance;
            var fillAmount = 0f;
            switch (_achievementData.Requirement.Key)
            {
                case RequirementType.EnemyKills:
                    fillAmount = saveFile.EnemiesKilled / requirementValue;
                    break;
                case RequirementType.Pickups:
                    fillAmount = saveFile.PickupsCollected / requirementValue;
                    break;
                case RequirementType.Shards when _achievementData.Character != CharactersEnum.Unknown:
                    if (_achievementData.Requirement.Value == 0)
                        fillAmount = saveFile.CharacterSaveData[_achievementData.Character].IsUnlocked ? 1 : 0;
                    else
                        fillAmount = saveFile.CharacterSaveData[_achievementData.Character].RankUpLevel / requirementValue;
                    break;
                case RequirementType.Shards:
                    fillAmount = saveFile.CharacterSaveData.Sum(x => x.Value.RankUpLevel) / requirementValue;
                    break;
                case RequirementType.EnemyKillInSingleGame:
                    fillAmount = saveFile.CharacterSaveData.Max(x => x.Value.KillCount) / requirementValue;
                    break;
                case RequirementType.LegendaryItems:
                    fillAmount = saveFile.TotalLegendaryItemsObtained / requirementValue;
                    break;
                case RequirementType.RegenerationInSingleGame:
                    fillAmount = saveFile.HealAmountInOneGame / requirementValue;
                    break;
                case RequirementType.DamageTakenInSingleGame:
                    fillAmount = saveFile.DamageTakeInOneGame / requirementValue;
                    break;
                case RequirementType.Deaths:
                    fillAmount = saveFile.Deaths / requirementValue;
                    break;
                case RequirementType.TimeSpendWithLightCharacter:
                    var lightCharacters = CharacterListManager.instance.GetCharacters().Where(x => x.Alignment == CharacterAlignment.Light).Select(x => x.Id).ToList();
                    fillAmount = saveFile.CharacterSaveData.Where(x => lightCharacters.Contains(x.Key)).Max(x => x.Value.LongestGameTime) / requirementValue;
                    break;
                case RequirementType.TimeSpendWithDarkCharacter:
                    var darkCharacters = CharacterListManager.instance.GetCharacters().Where(x => x.Alignment == CharacterAlignment.Dark).Select(x => x.Id).ToList();
                    fillAmount = saveFile.CharacterSaveData.Where(x => darkCharacters.Contains(x.Key)).Max(x => x.Value.LongestGameTime) / requirementValue;
                    break;
                case RequirementType.PlayTime when _achievementData.Character != CharactersEnum.Unknown:
                    fillAmount = saveFile.CharacterSaveData[_achievementData.Character].LongestGameTime / requirementValue;
                    break;
                case RequirementType.PlayTime:
                    fillAmount = saveFile.TimePlayed / requirementValue;
                    break;
                case RequirementType.EnemyBossKills:
                    fillAmount = saveFile.BossKills / requirementValue;
                    break;
                case RequirementType.HealAndDamageInSingleGame:
                    var fill1 = saveFile.HealAmountInOneGame / requirementValue;
                    var fill2 = saveFile.DamageTakeInOneGame / requirementValue;
                    fillAmount = fill1 / 2 + fill2 / 2;
                    break;
                case RequirementType.DistanceTraveled:
                    fillAmount = (float)(saveFile.DistanceTraveled / requirementValue);
                    break;
                case RequirementType.Shrines:
                    fillAmount = saveFile.ShrinesVisited / requirementValue;
                    break;
                case RequirementType.Reactions:
                    fillAmount = saveFile.ReactionsTriggered / requirementValue;
                    break;
                case RequirementType.AbyssFlowers:
                    fillAmount = saveFile.YamiFlowerPickup / requirementValue;
                    break;
                case RequirementType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            textCompletionPercentage.text = $"{fillAmount * 100:N0}%";
            progressBar.fillAmount = _isUnlocked ? 1 : fillAmount;
            _filterState = AchievementState.All;
        }
    }
}