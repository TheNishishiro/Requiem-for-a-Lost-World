using DefaultNamespace.Data;
using Managers;
using Objects.Characters;
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
        

        public void SetDisplay(Color gachaColor, CharactersEnum characterId)
        {
            if (characterId != CharactersEnum.Unknown)
            {
                var characterData = CharacterListManager.instance.GetCharacter(characterId);
                var characterSaveData = SaveFile.Instance.GetCharacterSaveData(characterId);

                var isConvertIntoGems = characterSaveData.IsUnlocked && characterSaveData.GetRankEnum() >= CharacterRank.E5;
                imageCharacter.sprite = characterData.CharacterCard;
                imageCharacter.rectTransform.anchoredPosition = new Vector2(characterData.GachaSubArtOffset.x,0);
                textRewardTitle.text = characterData.Name;
                textDescription.text = !characterSaveData.IsUnlocked ? "<color=yellow>NEW</color>" :
                    characterSaveData.GetRankEnum() < CharacterRank.E5 ? "Shard +1" : "Converted<br>Gems +200";
                if (isConvertIntoGems)
                    SaveFile.Instance.Gems += 200;

                SaveFile.Instance.UnlockCharacter(characterId);
            }
            else
            {
                switch (Random.value)
                {
                    case <= 0.5f:
                        imageCharacter.sprite = spriteGoldStack;
                        textRewardTitle.text = "Coins";
                        textDescription.text = "Gold +500";
                        SaveFile.Instance.Gold += 500;
                        break;
                    default:
                        imageCharacter.sprite = spriteGemPouch;
                        textRewardTitle.text = "Pouch";
                        textDescription.text = "Gem +150";
                        SaveFile.Instance.Gems += 150;
                        break;
                }
            }
            
            var particleMain = particles.main;
            particleMain.startColor = gachaColor;
            materialControllerFrontGlow.SetColor(gachaColor);
            imageLeftStrip.color = imageRightStrip.color = gachaColor;
            
            SaveManager.instance.SaveGame();
        }
    }
}