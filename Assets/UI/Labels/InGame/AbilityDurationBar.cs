using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Labels.InGame
{
	public class AbilityDurationBar : MonoBehaviour
	{
		[SerializeField] private Image imageIndicator;
		private float _value;
		private float _maxValue;
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
			_value = _maxValue = maxValue;
		}

		private void SetActive(bool isActive)
		{
			gameObject.SetActive(isActive);
		}

		private IEnumerator Tick()
		{
			while (_value > 0)
			{
				_value -= 0.05f;
				imageIndicator.fillAmount = _value / _maxValue;
				yield return new WaitForSeconds(0.05f);
			}

			SetActive(false);
			yield return null;
		}
	}
}