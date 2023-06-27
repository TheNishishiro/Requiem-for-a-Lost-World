using System;
using TMPro;
using UnityEngine;

namespace UI.Labels.InGame
{
	public class FpsCounter : MonoBehaviour
	{
		[SerializeField] private float _hudRefreshRate = 1f;

		private TextMeshProUGUI _fpsText;
		private float _timer;

		private void Awake()
		{
			_fpsText = GetComponent<TextMeshProUGUI>();
		}

		private void Update()
		{
			if (Time.unscaledTime <= _timer) return;
			
			var fps = (int)(1f / Time.unscaledDeltaTime);
			_fpsText.text = "FPS: " + fps;
			_timer = Time.unscaledTime + _hudRefreshRate;
		}
	}
}