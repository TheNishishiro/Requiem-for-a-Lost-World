using System;
using System.Collections;
using System.Collections.Generic;
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
		private List<Pickup> _expGem = new ();
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
				() => SpawnManager.instance.SpawnObject(transform.position, expShardPrefab).GetComponent<Pickup>(), 
				expGem =>
				{
					expGem.Reset();
					expGem.SetAmount(shardsAmount);
					expGem.transform.position = gemPosition;
					expGem.gameObject.SetActive(true);
					_expGem.Add(expGem);
				}, 
				expGem =>
				{
					expGem.gameObject.SetActive(false);
					_expGem.Remove(expGem);
				}, 
				expGem => Destroy(expGem.gameObject), true, 10000);
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
				Destroy(pickupObject.gameObject);
		}

		private IEnumerator MergeGemPickups()
		{
			while (true)
			{
				if (_expGem is not { Count: > 200 })
				{
					yield return new WaitForSeconds(5f);
					continue;
				}

				var planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

				for (var i = 0; i < _expGem.Count - 1; i++)
				{
					if (_expGem[i]?.gameObject.activeSelf != true)
						continue;
					
					// Check if gem i is within the camera's view
					if (GeometryUtility.TestPlanesAABB(planes, _expGem[i].boxCollider.bounds))
					{
						var distanceToPlayer = Vector3.Distance(_expGem[i].transform.position, player.transform.position);
						if (distanceToPlayer <= distanceFromPlayer)
							continue;
					}
						
					for (var j = i + 1; j < _expGem.Count; j++)
					{
						if (_expGem[j]?.gameObject.activeSelf != true)
							continue;

						var distance = Vector3.Distance(_expGem[i].transform.position, _expGem[j].transform.position);
						if (distance < mergingThreshold)
						{
							_expGem[i].GetExpObject().AddExp(_expGem[j].GetExpObject().expAmount);
							_gemPool.Release(_expGem[j]);
							j--;
						}
					}
				}
				

				yield return new WaitForSeconds(15f);
			}
		}
	}

}