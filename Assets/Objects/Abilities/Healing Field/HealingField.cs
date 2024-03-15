using System;
using Managers;
using Objects.Players.Scripts;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace Objects.Abilities.Healing_Field
{
	public class HealingField : PoolableProjectile<HealingField>
	{
		private float _healAmount;
		private bool _isEmpowering;

		public void Setup(float healAmount, bool isEmpowering)
		{
			_healAmount = healAmount;
			_isEmpowering = isEmpowering;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Player")) return;
			
			RpcManager.instance.HealPlayer(_healAmount, other.GetComponent<NetworkObject>().OwnerClientId);

			if (_isEmpowering)
				GameManager.instance.playerComponent.playerStatsComponent.TemporaryAttackBoost(0.5f, 1.5f);
		}
		
		protected override void Destroy()
		{
			ReturnToPool(_objectPool, _object);
		}
	}
}