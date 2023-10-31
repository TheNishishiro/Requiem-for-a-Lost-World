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
		[SerializeField] protected PickupObject pickUpObject;
		[SerializeField] private UnityEvent<PickupEnum> OnPickupAction;
		[SerializeField] protected PickupEnum PickupEnum;
		[SerializeField] public BoxCollider boxCollider;
		private PlayerStatsComponent _playerStatsComponent;
		private Player _character;
		protected bool IsFollowingPlayer;
		private const float Speed = 10f;
		[SerializeField] private bool isStationary;

		protected void Init()
		{
			_character = GameManager.instance.playerComponent;
			_playerStatsComponent = _character.playerStatsComponent;
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
				var distance = Vector3.Distance(_character.transform.position, transform.position);
				if (distance < _playerStatsComponent.GetMagnetSize())
					IsFollowingPlayer = true;
				return;
			}
		
			transform.position = Vector3.MoveTowards(transform.position, _character.transform.position, Speed * Time.deltaTime);
		}
		
		protected void OnCollision(Collider col)
		{
			if (col.gameObject.CompareTag("Player"))
			{
				var character = col.GetComponent<Player>();
				if (character is null)
					return;
				
				pickUpObject?.OnPickUp(character);
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