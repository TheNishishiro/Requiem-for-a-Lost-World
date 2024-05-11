using CarouselUI;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class SettingsEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [BoxGroup("Description")] [SerializeField] [Multiline] private string description;
        [BoxGroup("Description")] [SerializeField] private Sprite exampleImage;
        [SerializeField] private CarouselUIElement carousel;
        [SerializeField] private Image background;
        [SerializeField] private Color enabledColor;
        [SerializeField] private Color disabledColor;
        [SerializeField] private TMP_InputField textInput; 
        [SerializeField] private TextMeshProUGUI labelValue; 
        [SerializeField] private Slider sliderInput;
        private int _labelUnderlyingValue; 
        
        public void SetOptions(string[] options, int defaultSelection = 0)
        {
            carousel.SetOptions(options);
            carousel.UpdateIndex(defaultSelection);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SettingsScreenManager.instance.OpenDescription(description, exampleImage);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SettingsScreenManager.instance.OpenDescription(null, null);
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

        public void SetLabelValue(string text)
        {
            labelValue.text = text;
        }

        public void SetLabelValue(string text, int value)
        {
            labelValue.text = text;
            _labelUnderlyingValue = value;
        }

        public int GetLabelValue()
        {
            return _labelUnderlyingValue;
        }
    }
}