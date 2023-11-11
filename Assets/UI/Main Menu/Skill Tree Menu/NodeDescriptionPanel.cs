using Objects.Characters;
using TMPro;
using UnityEngine;

namespace UI.Main_Menu.Skill_Tree_Menu
{
    public class NodeDescriptionPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI descriptionText;

        private void Start()
        {
            descriptionText.text = string.Empty;
        }

        public void Open(string text)
        {
            descriptionText.text = text;
        }

        public void Clear()
        {
            descriptionText.text = string.Empty;
        }
    }
}