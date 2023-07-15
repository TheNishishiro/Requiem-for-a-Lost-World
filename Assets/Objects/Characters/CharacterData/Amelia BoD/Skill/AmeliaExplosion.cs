using System;
using DefaultNamespace;
using Objects.Players.Scripts;
using UnityEngine;

namespace Objects.Characters.Amelia.Skill
{
	public class AmeliaExplosion : MonoBehaviour
	{
		private PlayerStatsComponent _playerStatsComponent;
		public Vector3 maxScale = new Vector3(3, 3, 3);
		public float lifetime = 2f;
		public float scaleSpeed = 0.1f;

		private Vector3 initialScale = new Vector3(0.2f,0.2f,3);
		private float remainingTime = 2f;
		
		private void Start()
		{
			_playerStatsComponent = FindObjectOfType<PlayerStatsComponent>();
		}

		private void Update()
		{
			if (remainingTime > 0f)
			{
				float t = 1f - (remainingTime / lifetime); // Calculate the interpolation factor
				Vector3 newScale = Vector3.Lerp(initialScale, maxScale, t); // Interpolate the scale

				transform.localScale = newScale; // Apply the new scale

				remainingTime -= Time.deltaTime;
			}
			else
				transform.localScale = maxScale;
		}

		public void OnTriggerStay(Collider other)
		{
			if (Time.frameCount % 10 != 0)
				return;
			
			if (other.CompareTag("Enemy"))
			{
				other.GetComponent<Damageable>().TakeDamage(_playerStatsComponent.GetTotalDamage(25));
			}
		}
	}
}