using System;
using Managers;
using Unity.Netcode;
using UnityEngine;

namespace Objects.Enemies
{
	public class EnemyWinOnDeath : MonoBehaviour
	{
		public void OnDisable()
		{
			RpcManager.instance.TriggerWin();
		}
	}
}