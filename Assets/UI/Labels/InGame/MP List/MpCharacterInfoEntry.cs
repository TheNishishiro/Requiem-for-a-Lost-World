using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Labels.InGame.MP_List
{
    public class MpCharacterInfoEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nicknameText;
        [SerializeField] private ExperienceBar healthBar;
        [SerializeField] private Image avatar;

        public void SetData(float currentHp, float maxHp, int level, string playerName)
        {
            nicknameText.text = playerName;
            healthBar.UpdateSlider(currentHp, maxHp);
            healthBar.SetLevelText(level);
        }

        public void SetAvatar(Sprite characterAvatar)
        {
            avatar.sprite = characterAvatar;
        }
    }
}