using CarouselUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class SettingsEntry : MonoBehaviour
    {
        [SerializeField] private CarouselUIElement carousel;
        [SerializeField] private Image background;
        [SerializeField] private Color enabledColor;
        [SerializeField] private Color disabledColor;
        
        public void SetOptions(string[] options, int defaultSelection = 0)
        {
            carousel.SetOptions(options);
            carousel.UpdateIndex(defaultSelection);
        }

        public void SetSelection(int selectedOption)
        {
            carousel.UpdateIndex(selectedOption);
        }

        public int GetSelectedOption()
        {
            return carousel.CurrentIndex;
        }

        public void Next()
        {
            carousel.PressNext();
        }

        public void Previous()
        {
            carousel.PressPrevious();
        }

        public void SetActive(bool isActive)
        {
            background.color = isActive ? enabledColor : disabledColor;
        }
    }
}