using System;
using System.Collections;
using Objects.Drops.ExpDrop;
using UnityEngine;

namespace Managers
{
	public class PickupManager : MonoBehaviour
	{
		[SerializeField] int mergingThreshold;
		[SerializeField] private int distanceFromPlayer;
		[SerializeField] Camera mainCamera;
		private Player player;

		private void Awake()
		{
			player = FindObjectOfType<Player>();
			StartCoroutine(MergeGemPickups());
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