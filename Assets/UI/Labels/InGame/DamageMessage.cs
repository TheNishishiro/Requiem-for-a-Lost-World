using System;
using TMPro;
using UnityEngine;

namespace UI.Labels.InGame
{
	public class DamageMessage : MonoBehaviour
	{
		private float _timeToLive = 1f;
		private float _moveSpeed = 0.5f;

		private void OnEnable()
		{
			_timeToLive = 1f;
		}

		private void Update()
		{
			_timeToLive -= Time.deltaTime;
			if (_timeToLive <= 0)
				gameObject.SetActive(false);

			transform.position += Vector3.up * (_moveSpeed * Time.deltaTime);
		}
	}
}