using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Interfaces;
using Managers;
using NaughtyAttributes;
using Objects.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class AchievementScreenManager : MonoBehaviour, IStackableWindow
    {
        [BoxGroup("Prefabs")] [SerializeField] private AchievementCard achievementCardPrefab;
        [Space]
        [BoxGroup("Section Containers")] [SerializeField] private Transform containerCharacter;
        [BoxGroup("Section Containers")] [SerializeField] private Transform containerCombat;
        [BoxGroup("Section Containers")] [SerializeField] private Transform containerCollection;
        [BoxGroup("Section Containers")] [SerializeField] private Transform containerEnvironment;
        [BoxGroup("Section Containers")] [SerializeField] private Transform containerSurvival;
        [BoxGroup("Section Containers")] [SerializeField] private Transform containerMisc;
        [Space]
        [BoxGroup("Section Labels")] [SerializeField] private GameObject labelCharacter;
        [BoxGroup("Section Labels")] [SerializeField] private GameObject labelCombat;
        [BoxGroup("Section Labels")] [SerializeField] private GameObject labelCollection;
        [BoxGroup("Section Labels")] [SerializeField] private GameObject labelEnvironment;
        [BoxGroup("Section Labels")] [SerializeField] private GameObject labelSurvival;
        [BoxGroup("Section Labels")] [SerializeField] private GameObject labelMisc;
        [Space]
        [BoxGroup("Buttons")] [SerializeField] private List<Button> buttonSections;
        [BoxGroup("Buttons")] [SerializeField] private List<Button> buttonStates;
        [Space]
        [BoxGroup("Styling")] [SerializeField] private Material materialSelectedText;
        [BoxGroup("Styling")] [SerializeField] private Material materialIdleText;
        [BoxGroup("Styling")] [SerializeField] private Color colorHighlight;
        [Space]
        [BoxGroup("Side Panel")] [SerializeField] private GameObject sidePanel;
        [BoxGroup("Side Panel")] [SerializeField] private TextMeshProUGUI labelTitle;
        [BoxGroup("Side Panel")] [SerializeField] private TextMeshProUGUI labelDescription;
        [BoxGroup("Side Panel")] [SerializeField] private TextMeshProUGUI labelRewards;
        
        private List<AchievementCard> _achievementCards;
        private int currentSectionId;
        private int currentStateId;
        private const float KeyHoldDelay = 0.25f;
        private float _keyNextActionTime = 0f;
        private bool _isSubsectionOpened;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Close();
            
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) 
                _keyNextActionTime = 0;
            
            if (Input.GetKey(KeyCode.UpArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                currentSectionId--;
                if (currentSectionId < 0) currentSectionId = buttonSections.Count - 1;
                FilterSection(currentSectionId);
            }
            else if (Input.GetKey(KeyCode.DownArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                currentSectionId++;
                if (currentSectionId >= buttonSections.Count) currentSectionId = 0;
                FilterSection(currentSectionId);
            }                
            else if (Input.GetKey(KeyCode.LeftArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                currentStateId--;
                if (currentStateId < 0) currentStateId = buttonStates.Count - 1;
                FilterState(currentStateId);
            }
            else if (Input.GetKey(KeyCode.RightArrow) && Time.time > _keyNextActionTime)
            {
                _keyNextActionTime = Time.time + KeyHoldDelay;
                currentStateId++;
                if (currentStateId >= buttonStates.Count) currentStateId = 0;
                FilterState(currentStateId);
            }    
        }

        public void FilterSection(int sectionId)
        {
            currentSectionId = sectionId;
            var section = (AchievementSection)sectionId;
            labelCharacter.SetActive(section is AchievementSection.Character or AchievementSection.None);
            labelCombat.SetActive(section is AchievementSection.Combat or AchievementSection.None);
            labelCollection.SetActive(section is AchievementSection.Collection or AchievementSection.None);
            labelEnvironment.SetActive(section is AchievementSection.Environment or AchievementSection.None);
            labelSurvival.SetActive(section is AchievementSection.CharacterSurvival or AchievementSection.None);
            labelMisc.SetActive(section is AchievementSection.Misc or AchievementSection.None);
            
            containerCharacter.gameObject.SetActive(section is AchievementSection.Character or AchievementSection.None);
            containerCombat.gameObject.SetActive(section is AchievementSection.Combat or AchievementSection.None);
            containerCollection.gameObject.SetActive(section is AchievementSection.Collection or AchievementSection.None);
            containerEnvironment.gameObject.SetActive(section is AchievementSection.Environment or AchievementSection.None);
            containerSurvival.gameObject.SetActive(section is AchievementSection.CharacterSurvival or AchievementSection.None);
            containerMisc.gameObject.SetActive(section is AchievementSection.Misc or AchievementSection.None);
            
            buttonSections.ForEach(x => x.GetComponent<Image>().color = Color.clear);
            buttonSections.ForEach(x => x.GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial = materialIdleText);
            buttonSections[sectionId].GetComponent<Image>().color = colorHighlight;
            buttonSections[sectionId].GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial = materialSelectedText;
        }

        public void FilterState(int stateId)
        {
            currentStateId = stateId;
            var state = (AchievementState)stateId;

            buttonStates.ForEach(x => x.GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial = materialIdleText);
            buttonStates[stateId].GetComponentInChildren<TextMeshProUGUI>().fontSharedMaterial = materialSelectedText;
            
            _achievementCards.ForEach(x => x.VisibleState(state));
        }

        public void OpenAchievementDisplay(AchievementValueAttribute achievementData, IPlayerItem unlocksItem)
        {
            sidePanel.gameObject.SetActive(true);
            labelTitle.text = achievementData.Title;
            labelDescription.text = achievementData.Description;
            switch (achievementData.Reward.Key)
            {
                case RewardType.None when unlocksItem == null:
                    labelRewards.text = "None";
                    break;
                case RewardType.None:
                    labelRewards.text = "";
                    break;
                case RewardType.Gems:
                    labelRewards.text = $"Gems: {achievementData.Reward.Value}<br>";
                    break;
                case RewardType.Coins:
                    labelRewards.text = $"Coins: {achievementData.Reward.Value}<br>";
                    break;
                case RewardType.Shards:
                    var character = CharacterListManager.instance.GetCharacter((CharactersEnum)achievementData.Reward.Value);
                    labelRewards.text = $"Character shard: {character.Name}<br>";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (unlocksItem != null)
            {
                var title = unlocksItem.IsItem ? "Item" : "Weapon";
                labelRewards.text += $"{title}: {unlocksItem.NameField}";
            }
        }

        public void CloseAchievementDisplay()
        {
            sidePanel.gameObject.SetActive(false);
        }

        public void Open()
        {
            _achievementCards ??= new List<AchievementCard>();
            if (!_achievementCards.Any())
            {
                foreach (var achievement in 
                         Enum.GetValues(typeof(AchievementEnum)).Cast<AchievementEnum>()
                             .Select(x => new { id = x, achievementValue = x.GetAchievementValue()})
                             .OrderBy(x => x.achievementValue.Character)
                             .ThenBy(x => x.achievementValue.Rarity)
                             .ThenBy(x => x.achievementValue.Title)
                         )
                {
                    var container = achievement.achievementValue.Section switch
                    {
                        AchievementSection.Character => containerCharacter,
                        AchievementSection.Environment => containerEnvironment,
                        AchievementSection.Combat => containerCombat,
                        AchievementSection.Collection => containerCollection,
                        AchievementSection.CharacterSurvival => containerSurvival,
                        AchievementSection.Misc => containerMisc,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    var achievementCard = Instantiate(achievementCardPrefab, container);
                    achievementCard.Setup(achievement.achievementValue, achievement.id);
                    _achievementCards.Add(achievementCard);
                }
            }
            else
            {
                _achievementCards.ForEach(x => x.Refresh());
            }

            FilterSection(0);
            FilterState(0);
            CloseAchievementDisplay();
            StackableWindowManager.instance.OpenWindow(this);
        }
        
        public void Close()
        {
            StackableWindowManager.instance.CloseWindow(this);
        }

        public bool IsInFocus { get; set; }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}