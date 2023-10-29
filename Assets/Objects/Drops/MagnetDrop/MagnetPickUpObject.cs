using System.Collections;
using Interfaces;
using UnityEngine;

namespace Objects.Drops.MagnetDrop
{
	public class MagnetPickUpObject : PickupObject
	{
		public override void OnPickUp(Player player)
		{
			StartCoroutine(PullPickupsToPlayer());
		}

		public override void SetAmount(int amount)
		{
		}

		private static IEnumerator PullPickupsToPlayer()
		{
			var pickups = FindObjectsOfType<Pickup>();
			foreach (var pickup in pickups)
			{
				pickup.SetIsFollowingPlayer(true);
			}
			yield break;
		}
	}
}