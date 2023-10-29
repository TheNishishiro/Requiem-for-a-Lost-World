using System;
using Interfaces;
using Managers;
using Objects.Drops.ExpDrop;
using Objects.Players.Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.Drops
{
	public class PickupBase : MonoBehaviour
	{
		[SerializeField] private UnityEvent<PickupEnum> OnPickupAction;
		[SerializeField] protected PickupEnum PickupEnum;
		private PlayerStatsComponent PlayerStatsComponent;
		private Player Character;
		protected bool IsFollowingPlayer;
		private const float Speed = 10f;
		[SerializeField] private bool isStationary;

		protected void Init()
		{
			Character = FindObjectOfType<Player>();
			PlayerStatsComponent = FindObjectOfType<PlayerStatsComponent>();
		}

		public void Reset()
		{
			IsFollowingPlayer = false;
		}

		protected void FollowPlayerWhenClose()
		{
			if (isStationary)
				return;
			
			if (Time.frameCount % 2 != 0)
				return;
		
			if (!IsFollowingPlayer)
			{
				var distance = Vector3.Distance(Character.transform.position, transform.position);
				if (distance < PlayerStatsComponent.GetMagnetSize())
					IsFollowingPlayer = true;
				return;
			}
		
			transform.position = Vector3.MoveTowards(transform.position, Character.transform.position, Speed * Time.deltaTime);
		}
		
		protected void OnCollision(Collider col)
		{
			if (col.gameObject.CompareTag("Player"))
			{
				var character = col.GetComponent<Player>();
				if (character is null)
					return;
						
				var pickupObject = GetComponent<PickupObject>();
				pickupObject?.OnPickUp(character);
				OnPickupAction?.Invoke(PickupEnum);
				Destroy();
			}
		}

		protected virtual void Destroy()
		{
			Destroy(gameObject);
		}
	}
}