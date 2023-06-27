using Interfaces;
using UnityEngine;

namespace Objects.Drops.World_Pickups
{
	public class WorldPickup : PickupBase
	{
		private void Start()
		{
			Init();
		}

		private void Update()
		{
			FollowPlayerWhenClose();
		}

		private void OnTriggerEnter(Collider col)
		{
			OnCollision(col);
		}
	}
}