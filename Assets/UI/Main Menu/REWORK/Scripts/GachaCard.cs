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
        [SerializeField] private MaterialController materialController;
        [SerializeField] private Image imageCharacter;
        [SerializeField] private Image imageLeftStrip;
        [SerializeField] private Image imageRightStrip;
        [SerializeField] private TextMeshProUGUI textRewardTitle;
        [SerializeField] private TextMeshProUGUI textDescription;
        

        public void SetDisplay(Color gachaColor, CharactersEnum characterId)
        {
            if (characterId != CharactersEnum.Unknown)
            {
                var characterData = CharacterListManager.instance.GetCharacter(characterId);
                imageCharacter.sprite = characterData.FullArt;
                textRewardTitle.text = characterData.Name;
                textDescription.text = "";
            }

            materialController.SetColor(gachaColor);
            imageLeftStrip.color = imageRightStrip.color = gachaColor;
        }
    }
}