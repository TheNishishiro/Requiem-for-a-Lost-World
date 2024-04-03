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
        [SerializeField] private TMP_InputField textInput; 
        [SerializeField] private Slider sliderInput; 
        
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
            carousel?.PressNext();
        }

        public void Previous()
        {
            carousel?.PressPrevious();
        }

        public void SetActive(bool isActive)
        {
            background.color = isActive ? enabledColor : disabledColor;
        }

        public void SetText(string text)
        {
            textInput.text = text;
        }

        public string GetText()
        {
            return textInput.text;
        }

        public void SetSliderValue(float value)
        {
            sliderInput.value = value;
        }

        public float GetSliderValue()
        {
            return sliderInput.value;
        }
    }
}