using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Labels.InGame
{
	public class AbilityDurationBar : MonoBehaviour
	{
		[SerializeField] private Slider slider;
		private IEnumerator _tickCoroutine;

		public void StartTick(float maxValue)
		{
			SetMaxValue(maxValue);
			SetActive(true);
			if (_tickCoroutine != null)
				StopCoroutine(_tickCoroutine);


			_tickCoroutine = Tick();
			StartCoroutine(_tickCoroutine);
		}

		private void SetMaxValue(float maxValue)
		{
			slider.value = slider.maxValue = maxValue;
		}

		private void SetActive(bool isActive)
		{
			gameObject.SetActive(isActive);
		}

		private IEnumerator Tick()
		{
			while (slider.value > 0)
			{
				slider.value -= 0.1f;
				yield return new WaitForSeconds(0.1f);
			}

			SetActive(false);
			yield return null;
		}
	}
}