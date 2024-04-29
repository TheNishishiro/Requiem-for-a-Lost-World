using DefaultNamespace;
using DefaultNamespace.Data;
using Managers;
using Objects.Characters;
using Objects.Players.PermUpgrades;
using TMPro;
using UI.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class GachaCard : MonoBehaviour
    {
        [SerializeField] private MaterialController materialControllerFrontGlow;
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private Image imageCharacter;
        [SerializeField] private Image imageLeftStrip;
        [SerializeField] private Image imageRightStrip;
        [SerializeField] private TextMeshProUGUI textRewardTitle;
        [SerializeField] private TextMeshProUGUI textDescription;
        [SerializeField] private Sprite spriteGoldStack;
        [SerializeField] private Sprite spriteGemPouch;
        [SerializeField] private Sprite spriteRune;
        
        public void SetDisplay(Color displayColor, CharactersEnum characterId)
        {
            textDescription.color = Color.white;
            if (characterId != CharactersEnum.Unknown)
            {
                SetCharacterDisplay(characterId);
            }
            else
            {
                SetRandomDisplay();
            }
            
            var particleMain = particles.main;
            particleMain.startColor = displayColor;
            materialControllerFrontGlow.SetColor(displayColor);
            imageLeftStrip.color = imageRightStrip.color = displayColor;
            SaveManager.instance.SaveGame();
        }
        
        private void SetCharacterDisplay(CharactersEnum characterId)
        {
            var characterData = CharacterListManager.instance.GetCharacter(characterId);
            var characterSaveData = SaveFile.Instance.GetCharacterSaveData(characterId);
        
            var isConvertIntoGems = characterSaveData.IsUnlocked && characterSaveData.GetRankEnum() >= CharacterRank.E5;
            SetCharacterCommonDisplayProperties(characterData);
            
            textDescription.text = !characterSaveData.IsUnlocked ? "<color=yellow>NEW</color>" :
                characterSaveData.GetRankEnum() < CharacterRank.E5 ? "Shard +1" : "Converted<br>Gems +200";
        
            if (!isConvertIntoGems)
                SaveFile.Instance.UnlockCharacter(characterId);
            else
                AddGems(200);
        }
        
        private void SetRandomDisplay()
        {
            switch (Random.value)
            {
                case <= 0.2f:
                    SetRandomCharacterFragmentReward();
                    break;
                case <= 0.45f:
                    SetGemReward("Pouch", 150);
                    break;
                case <= 0.75f:
                    SetGoldReward("Coins", 500);
                    break;
                default:
                    SetRuneReward("Rune");
                    break;
            }
        }
        
        private void SetRandomCharacterFragmentReward()
        {
            var character = CharacterListManager.instance.GetRandomCharacter();
            var characterSaveData = SaveFile.Instance.CharacterSaveData[character.Id];
            if (characterSaveData.GetRankEnum() >= CharacterRank.E5)
            {
                switch (Random.value)
                {
                    case <= 0.33f:
                        SetGemReward("Pouch", 150);
                        break;
                    case <= 0.66f:
                        SetGoldReward("Coins", 500);
                        break;
                    default:
                        SetRuneReward("Rune");
                        break;
                }
            }
            else
            {
                SetCharacterCommonDisplayProperties(character);
        
                var fragmentReward = Random.Range(1, 6);
                textDescription.text = $"Fragments +{fragmentReward}";
                characterSaveData.AddFragments(fragmentReward);
            }
        }
        
        private void SetCharacterCommonDisplayProperties(CharacterData character)
        {
            imageCharacter.sprite = character.CharacterCard;
            imageCharacter.rectTransform.anchoredPosition = new Vector2(character.GachaSubArtOffset.x,0);
            textRewardTitle.text = character.Name;
        }
        
        private void SetGoldReward(string title, ulong goldAmount)
        {
            imageCharacter.sprite = spriteGoldStack;
            textRewardTitle.text = title;
            textDescription.text = $"Gold +{goldAmount}";
            SaveFile.Instance.Gold += goldAmount;
        }
        
        private void SetGemReward(string title, ulong gemAmount)
        {
            imageCharacter.sprite = spriteGemPouch;
            textRewardTitle.text = title;
            textDescription.text = $"Gem +{gemAmount}";
            AddGems(gemAmount);
        }
        
        private void SetRuneReward(string title)
        {
            var runeData = RuneListManager.instance.GetRandomRune();
            var runeSaveData = runeData.ResolveRune();
            var runeValue = runeData.GetScaledValue(runeSaveData);
            var displayValue = runeSaveData.statType.IsPercent() ? $"{runeValue*100:0.##}%" : $"{runeValue:0.##}";
            
            imageCharacter.sprite = spriteRune;
            textRewardTitle.text = title;
            textDescription.text = $"{runeData.statType.GetLongName()} +{displayValue}" ;
            textDescription.color = Utilities.RarityToColor(runeSaveData.rarity);
            SaveFile.Instance.AddRune(runeSaveData);
        }
        
        private void AddGems(ulong amount)
        {
            SaveFile.Instance.Gems += amount;
        }
    }
}