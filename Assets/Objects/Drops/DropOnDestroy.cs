using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Managers;
using Objects.Drops;
using Objects.Players.Scripts;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
	public class DropOnDestroy : MonoBehaviour
	{
		[SerializeField] private List<ChanceDrop> pickup;
		[SerializeField] private bool scaleWithLuck;
		private bool isQuitting;

		
		private void OnApplicationQuit()
		{
			isQuitting = true;
		}

		public void CheckDrop()
		{
			if (isQuitting)
				return;

			if (pickup?.Any() != true)
				return;
			
			var itemsToDrop = pickup.Where(x => x.chance >= Random.value - (scaleWithLuck ?PlayerStatsScaler.GetScaler().GetLuck() : 0));

			foreach (var item in itemsToDrop)
			{
				var randomPoint = Utilities.GetRandomInAreaFreezeParameter(transform.position, 0.05f, isFreezeY: true);
				var positionOnGround = Utilities.GetPointOnColliderSurface(randomPoint, transform, 0.25f);
				PickupManager.instance.SpawnPickup(positionOnGround, item.pickupObject, Random.Range(item.amountMin, item.amountMax));
			}
		}

		public void AddDrop(ChanceDrop chanceDrop)
		{
			pickup ??= new List<ChanceDrop>();
			pickup.Add(chanceDrop);
		}

		public void ClearDrop()
		{
			pickup ??= new List<ChanceDrop>();
			pickup.Clear();
		}
	}
}