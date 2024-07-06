using System;
using UnityEngine;

namespace Objects.Abilities.Reality_Crack
{
	public class ShatterPiece : MonoBehaviour
	{
		private Material _material;
		
		private const float FresnelStartValue = 10f;
		private const float FresnelEndValue = 0f;
		public float ShardDuration = 0.8f;
		private float _timer = 0f;
		private float _stepTime = 0f;

		public void Start()
		{
			_material = GetComponent<Renderer>().material;
			_material.SetFloat("_Fresnel", FresnelStartValue);
		}
		
		public void Update()
		{
			_timer += Time.deltaTime;
			_stepTime += Time.deltaTime;
			if (_stepTime < 0.2f)
				return;
			_stepTime = 0;
			
			var t = Mathf.Clamp01(_timer / ShardDuration);
			var fresnelValue = Mathf.Lerp(FresnelStartValue, FresnelEndValue, t);
			_material.SetFloat("_Fresnel", fresnelValue);
		}
	}
}