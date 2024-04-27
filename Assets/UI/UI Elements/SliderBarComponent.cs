using UnityEngine;
using UnityEngine.UI;

namespace UI.UI_Elements
{
    public class SliderBarComponent : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Image fillImage;

        public void SetValue(int value, int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = value;
        }
		
        public void SetValue(float value, float maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = value;
        }
		
        public void SetColor(Color color)
        {
            fillImage.color = color;
        }
    }
}