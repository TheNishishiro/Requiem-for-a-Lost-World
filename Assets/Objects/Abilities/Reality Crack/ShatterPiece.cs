using System;
using UnityEngine;

namespace Objects.Abilities.Reality_Crack
{
	public class ShatterPiece : MonoBehaviour
	{
		private Rigidbody _shardBody;
		private Material _material;
		private const float FresnelStartValue = 10f;
		private const float FresnelEndValue = 0f;
		public float ShardDuration = 0.8f;
		private float _timer = 0f;
		private float _stepTime = 0f;
		private Vector3 _startPosition = Vector3.zero;
		private Quaternion _startRotation;

		public void Start()
		{
			_material = GetComponent<Renderer>().material;
			_material.SetFloat("_Fresnel", FresnelStartValue);
			SetOrSavePosition();
		}

		private void OnEnable()
		{
			SetOrSavePosition();
			ResetRigidBody();
			_material = GetComponent<Renderer>().material;
			_material.SetFloat("_Fresnel", FresnelStartValue);
			_stepTime = 0;
			_timer = 0;
		}

		private void SetOrSavePosition()
		{
			if (_startPosition == Vector3.zero)
			{
				_startPosition = transform.localPosition;
				_startRotation = transform.localRotation;
			}
			else
			{
				transform.localPosition = _startPosition;
				transform.localRotation = _startRotation;
			}
		}

		private void ResetRigidBody()
		{
			_shardBody = GetComponent<Rigidbody>();
			_shardBody.linearVelocity = Vector3.zero; 
			_shardBody.angularVelocity = Vector3.zero; 
			_shardBody.useGravity = false;
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