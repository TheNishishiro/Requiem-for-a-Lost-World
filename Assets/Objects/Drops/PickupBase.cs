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
		[SerializeField] protected PickupEnum PickupEnum;
		[SerializeField] public BoxCollider boxCollider;
		private PlayerStatsComponent _playerStatsComponent;
		private Player _player;
		private Transform _cachedTransform;
		protected bool IsFollowingPlayer;
		private const float Speed = 20f;
		[SerializeField] private bool isStationary;

		protected void Init()
		{
			_player = GameManager.instance.playerComponent;
			_playerStatsComponent = _player.playerStatsComponent;
			_cachedTransform = transform;
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
				var distance = Vector3.Distance(_player.playerTransform.position, _cachedTransform.position);
				if (distance < PlayerStatsScaler.GetScaler().GetMagnetSize())
					IsFollowingPlayer = true;
				return;
			}
		
			_cachedTransform.position = Vector3.MoveTowards(_cachedTransform.position, _player.playerTransform.position, Speed * Time.deltaTime);
		}
		
		protected void OnCollision(Collider col)
		{
			if (col.gameObject.CompareTag("Player"))
			{
				var character = col.GetComponent<Player>();
				if (character is null)
					return;
				
				pickUpObject?.OnPickUp(character);
				AchievementManager.instance.OnPickupCollected(PickupEnum);
				Destroy();
			}
		}

		protected virtual void Destroy()
		{
			Destroy(gameObject);
		}
	}
}