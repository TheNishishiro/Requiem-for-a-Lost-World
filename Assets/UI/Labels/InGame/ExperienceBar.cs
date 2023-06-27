using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Labels.InGame
{
	public class ExperienceBar : MonoBehaviour
	{
		[SerializeField] private Slider slider;
		[SerializeField] private TextMeshProUGUI levelText;

		public void UpdateSlider(int value, int maxValue)
		{
			slider.maxValue = maxValue;
			slider.value = value;
		}

		public void SetLevelText(int level)
		{
			levelText.text = $"Lv. {level}";
		}
	}
}