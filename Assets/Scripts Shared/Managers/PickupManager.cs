using System;
using System.Collections;
using Interfaces;
using Objects.Drops;
using Objects.Drops.ExpDrop;
using UnityEngine;
using UnityEngine.Pool;

namespace Managers
{
	public class PickupManager : MonoBehaviour
	{
		public static PickupManager instance;
		[SerializeField] int mergingThreshold;
		[SerializeField] private int distanceFromPlayer;
		[SerializeField] Camera mainCamera;
		private Player player;
		private ObjectPool<Pickup> _gemPool;
		[SerializeField] private GameObject expShardPrefab;
		private int shardsAmount;
		private Vector3 gemPosition;

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}

			_gemPool = new ObjectPool<Pickup>(
				() => SpawnManager.instance.SpawnObject(transform.position, expShardPrefab).GetComponent<Pickup>(), expGem =>
				{
					expGem.Reset();
					expGem.SetAmount(shardsAmount);
					expGem.transform.position = gemPosition;
					expGem.gameObject.SetActive(true);
				}, 
				expGem => expGem.gameObject.SetActive(false), 
				Destroy, true, 10000);
			player = FindFirstObjectByType<Player>();
			StartCoroutine(MergeGemPickups());
		}

		public void SpawnPickup(Vector3 position, Pickup pickupObject, int amount)
		{
			if (pickupObject.PickupType == PickupEnum.Experience)
			{
				shardsAmount = amount;
				gemPosition = position;
				_gemPool.Get();
			}
			else
			{
				var spawnedObject = SpawnManager.instance.SpawnObject(position, pickupObject.gameObject).GetComponent<Pickup>();
				spawnedObject.SetAmount(amount);
			}
		}

		public void DestroyPickup(Pickup pickupObject)
		{
			if (pickupObject.PickupType == PickupEnum.Experience)
				_gemPool.Release(pickupObject);
			else
				Destroy(pickupObject);
		}

		private IEnumerator MergeGemPickups()
		{
			while (true)
			{
				var gems = FindObjectsOfType<ExpPickUpObject>();
				if (gems == null || gems.Length <= 200)
				{
					yield return new WaitForSeconds(5f);
					continue;
				}

				var planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

				for (var i = 0; i < gems.Length - 1; i++)
				{
					if (gems[i] == null)
						continue;
					
					// Check if gem i is within the camera's view
					if (GeometryUtility.TestPlanesAABB(planes, gems[i].GetComponent<Collider>().bounds))
					{
						var distanceToPlayer = Vector3.Distance(gems[i].transform.position, player.transform.position);
						if (distanceToPlayer <= distanceFromPlayer)
							continue;
					}
						
					for (var j = i + 1; j < gems.Length; j++)
					{
						if (gems[j] == null)
							continue;

						var distance = Vector3.Distance(gems[i].transform.position, gems[j].transform.position);
						if (distance < mergingThreshold)
						{
							gems[i].AddExp(gems[j].expAmount);
							Destroy(gems[j].gameObject);
							gems[j] = null;
						}
					}
				}
				

				yield return new WaitForSeconds(15f);
			}
		}
	}

}