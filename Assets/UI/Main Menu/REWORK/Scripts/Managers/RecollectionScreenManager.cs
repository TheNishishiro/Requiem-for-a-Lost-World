using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Gacha;
using DefaultNamespace.Extensions;
using Interfaces;
using Managers;
using NaughtyAttributes;
using Objects.Characters;
using TMPro;
using UI.Shared;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class RecollectionScreenManager : MonoBehaviour, IStackableWindow
    {
        [BoxGroup("Image")] [SerializeField] private Image imageBannerCharacter;
        [BoxGroup("Image")] [SerializeField] private Image imageSubBanner1;
        [BoxGroup("Image")] [SerializeField] private Image imageSubBanner2;
        [BoxGroup("Image")] [SerializeField] private Image imageSubBanner3;
        [BoxGroup("Image")] [SerializeField] private Image imageTimeTheme;
        [BoxGroup("Image")] [SerializeField] private Image imagePityTheme;
        [BoxGroup("Image")] [SerializeField] private Image imageGemsTheme;
        [BoxGroup("Image")] [SerializeField] private Image imagePull1;
        [BoxGroup("Image")] [SerializeField] private Image imagePull10;
        [BoxGroup("Image")] [SerializeField] private Image imageUnderscoreLine1;
        [BoxGroup("Image")] [SerializeField] private Image imageUnderscoreLine2;
        [BoxGroup("Image")] [SerializeField] private Image imageSubUnderscoreLine;
        [Space]
        [BoxGroup("Text")] [SerializeField] private TextMeshProUGUI textCharacterTitle;
        [BoxGroup("Text")] [SerializeField] private TextMeshProUGUI textCharacterName;
        [BoxGroup("Text")] [SerializeField] private TextMeshProUGUI textTimeLeft;
        [BoxGroup("Text")] [SerializeField] private TextMeshProUGUI textPity;
        [BoxGroup("Text")] [SerializeField] private TextMeshProUGUI textGems;
        [Space]
        [BoxGroup("Image")] [SerializeField] private MaterialController pullFlash;
        [BoxGroup("Pulls")] [SerializeField] private List<GachaShardComponent> gachaShards;
        [BoxGroup("Pulls")] [SerializeField] private List<GachaCard> gachaCards;
        [Space]
        [BoxGroup("Animator")] [SerializeField] private Animator animator;

        private GachaRewardType _highestRarity;
        private const int MaxPity = 40;

        public void Update()
        {
            if (!IsInFocus) return;
            
            if (Input.GetKeyDown(KeyCode.Escape))
                Close();
            
            var date1 = DateTime.Now;
            var date2 = SaveFile.Instance.NextBannerChangeDate.GetValueOrDefault();
            var difference = date2 - date1;
            var totalHoursLeft = (int)difference.TotalHours;
            var totalMinutesLeft = (int)difference.TotalMinutes;
            var displayMinutesLeft = totalMinutesLeft % 60; // get the "minute portion" of the total minutes left

            textTimeLeft.text = $"{totalHoursLeft}h {displayMinutesLeft}m";
            textPity.text = (MaxPity - SaveFile.Instance.Pity).ToString();
            textGems.text = SaveFile.Instance.Gems.ToString();
        }

        public void Pull(int amount)
        {
            var saveFile = SaveFile.Instance;
            var pullCost = (ulong)(amount == 1 ? 350 : 3000);
            if (saveFile.Gems < pullCost) return;
            saveFile.Gems -= pullCost;
            
            gachaShards.ForEach(x => x.gameObject.SetActive(false));
            gachaCards.ForEach(x => x.gameObject.SetActive(false));
            _highestRarity = GachaRewardType.Extra;
            for (var i = 0; i < amount; i++)
            {
                saveFile.Pity++;
                var pullDecision = Random.value switch
                {
                    < 0.1f => GachaRewardType.Main,
                    < 0.4f => GachaRewardType.Sub,
                    _ => GachaRewardType.Extra
                };
                if (_highestRarity > pullDecision)
                    _highestRarity = pullDecision;

                var pullColor = GetColorByRarity(pullDecision);
                var pulledCharacter = pullDecision switch
                {
                    GachaRewardType.Main => saveFile.CurrentBannerCharacterId,
                    GachaRewardType.Sub when Random.value <= 0.5 => GetAnyPromotionalCharacters(saveFile),
                    GachaRewardType.Sub => CharacterListManager.instance.GetCharacters().Where(x => x.Id != saveFile.CurrentBannerCharacterId).OrderBy(x => Random.value).First().Id,
                    _ => CharactersEnum.Unknown
                };
                if (saveFile.Pity >= MaxPity)
                {
                    pulledCharacter = saveFile.CurrentBannerCharacterId;
                    saveFile.Pity = 0;
                }

                AchievementManager.instance.OnPull(pulledCharacter);
                
                gachaCards[i].SetDisplay(pullColor, pulledCharacter);
                gachaCards[i].gameObject.SetActive(true);

                gachaShards[i].SetColor(pullColor);
                gachaShards[i].gameObject.SetActive(true);
            }
            
            pullFlash.SetColor(Color.white);
            animator.SetTrigger("Pull");
        }

        private CharactersEnum GetAnyPromotionalCharacters(SaveFile saveFile)
        {
            return Random.value switch
            {
                < 0.33f => saveFile.CurrentBannerSubCharacterId1,
                < 0.66f => saveFile.CurrentBannerSubCharacterId2,
                _ => saveFile.CurrentBannerSubCharacterId3
            };
        }

        public void ApplyPullColorToFlash()
        {
            var newColor = GetColorByRarity(_highestRarity);
            StartCoroutine(ChangeFlashColor(Color.white, newColor));
        }

        private IEnumerator ChangeFlashColor(Color baseColor, Color newColor)
        {
            var duration = 1.0f;
            var elapsed = 0.0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var t = elapsed / duration;

                var interpolatedColor = Color.Lerp(baseColor, newColor, t);
                pullFlash.SetColor(interpolatedColor);

                yield return null;
            }
        }

        private Color GetColorByRarity(GachaRewardType gachaRewardType)
        {
            return gachaRewardType switch
            {
                GachaRewardType.Unknown => Color.white,
                GachaRewardType.Main => Utilities.HexToColor("#FFF479"),
                GachaRewardType.Sub => Utilities.HexToColor("#F78AFF"),
                GachaRewardType.Extra => Utilities.HexToColor("#69B2FF"),
                _ => throw new ArgumentOutOfRangeException(nameof(gachaRewardType), gachaRewardType, null)
            };
        }

        public void Open()
        {
            var saveFile = SaveFile.Instance;
            var isRefreshBanner = saveFile.NextBannerChangeDate is null ||
                                  DateTime.Now > saveFile.NextBannerChangeDate;
            if (isRefreshBanner)
            {
                BuildBanner(saveFile);
            }

            var mainCharacter = CharacterListManager.instance.GetCharacter(saveFile.CurrentBannerCharacterId);
            imageBannerCharacter.sprite = mainCharacter.GatchaArt;
            imageTimeTheme.color = mainCharacter.ColorTheme;
            imagePityTheme.color = mainCharacter.ColorTheme;
            imagePull1.color = mainCharacter.ColorTheme;
            imagePull10.color = mainCharacter.ColorTheme;
            textCharacterTitle.color = mainCharacter.ColorTheme;
            textCharacterName.color = mainCharacter.ColorTheme;
            textTimeLeft.color = mainCharacter.ColorTheme;
            textPity.color = mainCharacter.ColorTheme;
            imageGemsTheme.color = mainCharacter.ColorTheme;
            imageUnderscoreLine1.color = mainCharacter.ColorTheme;
            imageUnderscoreLine2.color = mainCharacter.ColorTheme;
            imageSubUnderscoreLine.color = mainCharacter.ColorTheme;

            textCharacterName.text = mainCharacter.Name;
            textCharacterTitle.text = mainCharacter.Title;
            
            var subCharacter1 = CharacterListManager.instance.GetCharacter(saveFile.CurrentBannerSubCharacterId1);
            imageSubBanner1.rectTransform.anchoredPosition = new Vector2(subCharacter1.GachaSubArtOffset.x, subCharacter1.GachaSubArtOffset.y);
            imageSubBanner1.sprite = subCharacter1.CharacterCard;
            
            var subCharacter2 = CharacterListManager.instance.GetCharacter(saveFile.CurrentBannerSubCharacterId2);
            imageSubBanner2.rectTransform.anchoredPosition = new Vector2(subCharacter2.GachaSubArtOffset.x, subCharacter2.GachaSubArtOffset.y);
            imageSubBanner2.sprite = subCharacter2.CharacterCard;
            
            var subCharacter3 = CharacterListManager.instance.GetCharacter(saveFile.CurrentBannerSubCharacterId3);
            imageSubBanner3.rectTransform.anchoredPosition = new Vector2(subCharacter3.GachaSubArtOffset.x, subCharacter3.GachaSubArtOffset.y);
            imageSubBanner3.sprite = subCharacter3.CharacterCard;
            
            
            StackableWindowManager.instance.OpenWindow(this);
        }

        public void Close()
        {
            StackableWindowManager.instance.CloseWindow(this);
        }

        private static void BuildBanner(SaveFile saveFile)
        {
            var characters = CharacterListManager.instance.GetCharacters().Select(x => x.Id).ToList();
            var weightedCharacters = new List<CharactersEnum>();

            foreach (var character in characters)
            {
                var lastBannerDate = saveFile.GetCharacterSaveData(character).LastBannerDate;
                var daysSinceLastBannerAppearance = Math.Min((DateTime.Now - lastBannerDate).Days, 20);
                
                for (var i = 0; i < daysSinceLastBannerAppearance; i++)
                {
                    weightedCharacters.Add(character);
                }
            }    
        
            saveFile.CurrentBannerCharacterId = weightedCharacters.GetNextRandom();
            weightedCharacters.Remove(saveFile.CurrentBannerCharacterId);
            saveFile.GetCharacterSaveData(saveFile.CurrentBannerCharacterId).LastBannerDate = DateTime.Now;
            
            
            saveFile.CurrentBannerSubCharacterId1 = weightedCharacters.GetNextRandom();
            weightedCharacters.Remove(saveFile.CurrentBannerSubCharacterId1);
            saveFile.CurrentBannerSubCharacterId2 = weightedCharacters.GetNextRandom();
            weightedCharacters.Remove(saveFile.CurrentBannerSubCharacterId2);
            saveFile.CurrentBannerSubCharacterId3 = weightedCharacters.GetNextRandom();
            weightedCharacters.Remove(saveFile.CurrentBannerSubCharacterId3);
        
            var now = DateTime.Now;
            var diffInDays = 0;
            if (saveFile.NextBannerChangeDate.HasValue)
            {
                diffInDays = (int)(now - saveFile.NextBannerChangeDate.Value).TotalDays;
            }

            saveFile.Pity = 0;
            saveFile.NextBannerChangeDate = saveFile.NextBannerChangeDate?.AddDays(diffInDays + 1) ?? now.AddHours(24);
            saveFile.Save();
        }
        
        public bool IsInFocus { get; set; }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}