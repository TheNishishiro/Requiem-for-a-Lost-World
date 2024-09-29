using System;
using Managers;
using Unity.Netcode;
using UnityEngine;

namespace Objects.Drops.ChestDrop
{
	public class ChestDrop : NetworkBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (!other.gameObject.CompareTag("Player") || other.gameObject.GetComponent<NetworkObject>()?.IsOwner != true) return;
			
			ChestPanelManager.instance.OpenPanel();
			PickupManager.instance.DestroyPickup(GetComponent<Pickup>());
		}
	}
}