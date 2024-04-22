using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.In_Game.GUI.Scripts
{
    public class UiItemContainer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textLevel;
        [SerializeField] private Image imageIcon;

        public void Setup(IPlayerItem item)
        {
            textLevel.text = item.LevelField.ToString();
            imageIcon.sprite = item.IconField;
            gameObject.SetActive(true);
        }
    }
}