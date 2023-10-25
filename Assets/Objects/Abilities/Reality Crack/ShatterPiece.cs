using System;
using UnityEngine;

namespace Objects.Abilities.Reality_Crack
{
	public class ShatterPiece : MonoBehaviour
	{
		private Material _material;
		
		private const float FresnelStartValue = 10f;
		private const float FresnelEndValue = 0f;
		public float ShardDuration = 1f;
		private float _timer = 0f;

		public void Start()
		{
			_material = GetComponent<Renderer>().material;
		}

		public void Update()
		{
			_timer += Time.deltaTime;
			var t = Mathf.Clamp01(_timer / ShardDuration);
			var fresnelValue = Mathf.Lerp(FresnelStartValue, FresnelEndValue, t);
			_material.SetFloat("_Fresnel", fresnelValue);
		}
	}
}