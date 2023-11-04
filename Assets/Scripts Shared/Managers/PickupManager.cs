using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Interfaces;
using Objects.Drops;
using Objects.Drops.ExpDrop;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

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
					_expGem.Remove(expGem);
					expGem.gameObject.SetActive(false);
				}, 
				expGem =>
				{
					_expGem.Remove(expGem);
					Destroy(expGem.gameObject);
				}, true, 600);
			player = FindFirstObjectByType<Player>();
			StartCoroutine(MergeGemPickups());
		}

		public void SpawnPickup(Vector3 position, Pickup pickupObject, int amount)
		{
			if (pickupObject.PickupType == PickupEnum.Experience)
			{
				if (_expGem.Count < 400)
				{
					shardsAmount = amount;
					gemPosition = position;
					_gemPool.Get();
				}
				else
				{
					var expGem = _expGem.OrderBy(_ => Random.value).First().GetExpObject();
					expGem.AddExp(amount);
				}
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
				if (_expGem is not { Count: > 380 })
				{
					yield return new WaitForSeconds(5f);
					continue;
				}

				var testGems = _expGem
					.Where(x => 
						x.gameObject.activeSelf 
						&& !Utilities.IsWithinCameraView(mainCamera, x.boxCollider.bounds, x.transform.position, player.transform.position, distanceFromPlayer)
					)
					.Select(x =>
					new {
						Pickup = x,
						Position = x.transform.position
					})
					.ToList();

				for (var i = 0; i < testGems.Count - 1; i++)
				{
					if (testGems[i].Pickup.gameObject.activeSelf != true)
						continue;
						
					for (var j = i + 1; j < testGems.Count; j++)
					{
						if (testGems[j].Pickup.gameObject.activeSelf != true)
							continue;

						var distance = Vector3.Distance(testGems[i].Position, testGems[j].Position);
						if (distance < mergingThreshold)
						{
							testGems[i].Pickup.GetExpObject().AddExp(testGems[j].Pickup.GetExpObject().expAmount);
							_gemPool.Release(testGems[j].Pickup);
							j--;
						}
					}
				}
				
				yield return new WaitForSeconds(20f);
			}
		}
	}
}