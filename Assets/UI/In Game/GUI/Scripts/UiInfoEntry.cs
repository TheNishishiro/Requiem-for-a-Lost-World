using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace UI.In_Game.GUI.Scripts
{
    public class UiInfoEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private UILineRenderer lineRendererTheme;
        [SerializeField] private UICircle circle1Theme;
        [SerializeField] private UICircle circle2Theme;
        
        public void SetTheme(Color colorTheme)
        {
            lineRendererTheme.color = colorTheme;
            circle1Theme.color = colorTheme;
            circle2Theme.color = colorTheme;
        }

        public void SetText(int number)
        {
            text.text = number.ToString();
        }
    }
}