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
		private ObjectPool<Pickup> _shardPool;
		private ObjectPool<Pickup> _goldPool;
		private ObjectPool<Pickup> _gemPool;
		private List<Pickup> _expGem = new ();
		[SerializeField] private GameObject expShardPrefab;
		[SerializeField] private GameObject goldCoinPrefab;
		[SerializeField] private GameObject gemPrefab;
		private int pickupAmount;
		private Vector3 pickupPosition;

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}

			_shardPool = new ObjectPool<Pickup>(
				() => SpawnManager.instance.SpawnObject(transform.position, expShardPrefab).GetComponent<Pickup>(), 
				expGem =>
				{
					expGem.Reset();
					expGem.SetAmount(pickupAmount);
					expGem.transform.position = pickupPosition;
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
			
			_goldPool = new ObjectPool<Pickup>(
				() => SpawnManager.instance.SpawnObject(transform.position, goldCoinPrefab).GetComponent<Pickup>(), 
				goldCoin =>
				{
					goldCoin.Reset();
					goldCoin.SetAmount(pickupAmount);
					goldCoin.transform.position = pickupPosition;
					goldCoin.gameObject.SetActive(true);
				}, 
				goldCoin =>
				{
					goldCoin.gameObject.SetActive(false);
				}, 
				goldCoin =>
				{
					Destroy(goldCoin.gameObject);
				}, true, 100);
			
			_gemPool = new ObjectPool<Pickup>(
				() => SpawnManager.instance.SpawnObject(transform.position, gemPrefab).GetComponent<Pickup>(), 
				gem =>
				{
					gem.Reset();
					gem.SetAmount(pickupAmount);
					gem.transform.position = pickupPosition;
					gem.gameObject.SetActive(true);
				}, 
				gem =>
				{
					gem.gameObject.SetActive(false);
				}, 
				gem =>
				{
					Destroy(gem.gameObject);
				}, true, 100);
			
			
			player = FindFirstObjectByType<Player>();
			StartCoroutine(MergeGemPickups());
		}

		public void SpawnPickup(Vector3 position, Pickup pickupObject, int amount)
		{
			pickupAmount = amount;
			pickupPosition = position;
			switch (pickupObject.PickupType)
			{
				case PickupEnum.Experience when _expGem.Count < 400:
					_shardPool.Get();
					break;
				case PickupEnum.Experience:
				{
					var expGem = _expGem.OrderBy(_ => Random.value).First().GetExpObject();
					expGem.AddExp(amount);
					break;
				}
				case PickupEnum.Gold:
					_goldPool.Get();
					break;
				case PickupEnum.Gem:
					_gemPool.Get();
					break;
				default:
				{
					var spawnedObject = SpawnManager.instance.SpawnObject(position, pickupObject.gameObject).GetComponent<Pickup>();
					spawnedObject.SetAmount(amount);
					break;
				}
			}
		}

		public void DestroyPickup(Pickup pickupObject)
		{
			switch (pickupObject.PickupType)
			{
				case PickupEnum.Experience:
					_shardPool.Release(pickupObject);
					break;
				case PickupEnum.Gold:
					_goldPool.Release(pickupObject);
					break;
				case PickupEnum.Gem:
					_gemPool.Release(pickupObject);
					break;
				default:
					Destroy(pickupObject.gameObject);
					break;
			}
		}

		public void SummonToPlayer()
		{
			foreach (var expGem in _expGem)
			{
				expGem.SetIsFollowingPlayer(true);
			}
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
							_shardPool.Release(testGems[j].Pickup);
							j--;
						}
					}
				}
				
				yield return new WaitForSeconds(20f);
			}
		}
	}
}