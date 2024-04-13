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
using Random = System.Random;

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

        private readonly Random _rnd = new ();
        private GachaRewardType _highestRarity;
        private const int MaxPity = 40;

        public void Update()
        {
            var date1 = DateTime.Now;
            var date2 = SaveFile.Instance.LastBannerChangeDate.GetValueOrDefault().AddHours(24);
            var difference = date2 - date1;
            textTimeLeft.text = $"{difference.Hours}h {difference.Minutes}m";
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
                var pullDecision = _rnd.NextDouble() switch
                {
                    < 0.1f => GachaRewardType.Main,
                    < 0.5f => GachaRewardType.Sub,
                    <= 1f => GachaRewardType.Extra,
                    _ => throw new ArgumentOutOfRangeException()
                };
                if (_highestRarity > pullDecision)
                    _highestRarity = pullDecision;

                var pullColor = GetColorByRarity(pullDecision);
                var pulledCharacter = pullDecision switch
                {
                    GachaRewardType.Main => saveFile.CurrentBannerCharacterId,
                    GachaRewardType.Sub when _rnd.NextDouble() <= 0.5 => GetAnyPromotionalCharacters(saveFile),
                    GachaRewardType.Sub => CharacterListManager.instance.GetCharacters().Where(x => x.Id != saveFile.CurrentBannerCharacterId).OrderBy(x => _rnd.NextDouble()).First().Id,
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
            return _rnd.NextDouble() switch
            {
                < 0.33 => saveFile.CurrentBannerSubCharacterId1,
                < 0.66 => saveFile.CurrentBannerSubCharacterId2,
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
            var isRefreshBanner = saveFile.LastBannerChangeDate is null ||
                                  DateTime.Now.AddHours(-24) > saveFile.LastBannerChangeDate;
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
            saveFile.CurrentBannerCharacterId = characters.GetNextRandom();
            saveFile.CurrentBannerSubCharacterId1 = characters.GetNextRandom();
            saveFile.CurrentBannerSubCharacterId2 = characters.GetNextRandom();
            saveFile.CurrentBannerSubCharacterId3 = characters.GetNextRandom();
            saveFile.Pity = 0;
            saveFile.LastBannerChangeDate = saveFile.LastBannerChangeDate?.AddHours(24) < DateTime.Now
                ? DateTime.Now.AddHours(24)
                : saveFile.LastBannerChangeDate?.AddHours(24) ?? DateTime.Now;
            saveFile.Save();
        }
        
        public bool IsInFocus { get; set; }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}