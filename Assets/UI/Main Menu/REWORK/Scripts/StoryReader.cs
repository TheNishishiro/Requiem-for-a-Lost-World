using TMPro;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class StoryReader : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textContent;

        public void SetText(TextAsset textFile)
        {
            textContent.text = textFile.text;
        }
    }
}