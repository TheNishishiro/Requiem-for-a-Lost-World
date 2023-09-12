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
		private float elapsedTime = 0f;
		
		private void Start()
		{
			_playerStatsComponent = FindObjectOfType<PlayerStatsComponent>();
		}

		private void Update()
		{
			elapsedTime += Time.deltaTime * scaleSpeed;

			if (elapsedTime < lifetime)
			{
				float t = Mathf.Clamp01(elapsedTime / lifetime); // Clamp t to be between 0 and 1
				transform.localScale = Vector3.Lerp(initialScale, maxScale, t); // Interpolate the scale based on elapsed time
			}
			else
			{
				transform.localScale = maxScale; // Set to maxScale at the end of the lifetime
			}    
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