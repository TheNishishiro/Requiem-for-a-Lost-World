using System;
using System.Linq;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using Objects.Characters;
using TMPro;
using UnityEngine;
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
        private AchievementValueAttribute _achievementData;
        private AchievementEnum _achievementId;
        private AchievementSection _filterSection;
        private AchievementState _filterState;
        private bool _isUnlocked;
        
        public void Setup(AchievementValueAttribute achievementValue, AchievementEnum achievementEnum)
        {
            _achievementData = achievementValue;
            _achievementId = achievementEnum;
            
            title.text = achievementValue.Title;
            iconUnlockable.sprite = weaponContainer.GetIconByAchievement(achievementEnum) ?? itemContainer.GetIconByAchievement(achievementEnum);
            if (achievementValue.Character != CharactersEnum.Unknown && iconUnlockable.sprite == null)
                iconUnlockable.sprite = CharacterListManager.instance.GetCharacter(achievementValue.Character).Avatar;
            
            Refresh();
        }

        public void VisibleSection(AchievementSection section)
        {
            _filterSection = section;
            UpdateVisibility();
        }

        public void VisibleState(AchievementState state)
        {
            _filterState = state;
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            var filterUnlockState = _filterState == AchievementState.Completed;
            var isVisible = (_achievementData.Section == _filterSection || _filterSection == AchievementSection.None)
                             && (_isUnlocked == filterUnlockState || _filterState == AchievementState.All);
            
            gameObject.SetActive(isVisible);
        }

        public void OpenAchievementDisplay()
        {
            GetComponentInParent<AchievementScreenManager>().OpenAchievementDisplay(_achievementData);
        }

        public void Refresh()
        {
            var color = _achievementData.Rarity switch
            {
                Rarity.Common => Color.green,
                Rarity.Rare => Color.cyan,
                Rarity.Legendary => Color.yellow,
                Rarity.Mythic => Color.red,
                _ => throw new ArgumentOutOfRangeException()
            };

            SaveFile.Instance.AchievementSaveData.TryGetValue(_achievementId, out _isUnlocked);
            if (!_isUnlocked)
                color = Color.grey;
            background.color = progressBar.color = color;
            iconUnlockable.material = _isUnlocked ? null : materialBlackAndWhite;

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
                    fillAmount = saveFile.CharacterSaveData[_achievementData.Character].RankUpLevel + 1 / requirementValue;
                    break;
                case RequirementType.Shards:
                    fillAmount = saveFile.CharacterSaveData.Sum(x => x.Value.RankUpLevel) + 1 / requirementValue;
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
                case RequirementType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            progressBar.fillAmount = _isUnlocked ? 1 : fillAmount;
            
            
            
            _filterSection = AchievementSection.None;
            _filterState = AchievementState.All;
        }
    }
}