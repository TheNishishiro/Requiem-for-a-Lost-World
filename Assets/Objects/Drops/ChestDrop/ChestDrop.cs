using System;
using Managers;
using UnityEngine;

namespace Objects.Drops.ChestDrop
{
	public class ChestDrop : MonoBehaviour
	{
		private ChestPanelManager chestPanelManager;

		private void Start()
		{
			chestPanelManager = FindObjectOfType<ChestPanelManager>();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.gameObject.CompareTag("Player")) return;
			
			chestPanelManager.OpenPanel();
			Destroy(gameObject);
		}
	}
}