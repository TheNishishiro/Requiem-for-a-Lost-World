using System;
using System.Collections;
using Objects.Players.Scripts;
using UnityEngine;

namespace Objects.Enemies.Peace_Ender.Attacks
{
	public class LightPillarAttack : MonoBehaviour
	{
		private BoxCollider _collider;
		
		private void Awake()
		{
			_collider = GetComponent<BoxCollider>();
			StartCoroutine(Attack());
		}
		
		private IEnumerator Attack()
		{
			yield return new WaitForSeconds(0.2f);
			_collider.enabled = false;
			yield return new WaitForSeconds(0.3f);
			Destroy(gameObject);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
                other.GetComponent<Player>().TakeDamage(30);
			}
		}
	}
}