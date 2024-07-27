using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Labels.InGame
{
	public class SpecialBar : MonoBehaviour
	{
		[SerializeField] private Slider slider;
		
		public void SetValue(float value)
		{
			slider.value = value;
		}
		
		public void Increment(float value)
		{
			slider.value += value;
			if (slider.value > slider.maxValue)
				slider.value = slider.maxValue;
			else if (slider.value < 0)
				slider.value = 0;
		}

		public void SetMax(float maxValue)
		{
			slider.maxValue = maxValue;
		}
		
		public bool IsFull()
		{
			return Math.Abs(slider.value - slider.maxValue) < 0.01f && slider.maxValue != 0;
		}
		
		public float GetPercentage()
		{
			return slider.value / slider.maxValue;
		}
	}
}