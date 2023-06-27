using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.Character_List_Menu
{
	public class CharacterExpBar : MonoBehaviour
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