using System;
using Managers;
using Objects.Stage;
using Unity.Netcode;
using UnityEngine;

namespace Objects.Enemies
{
	public class EnemyWinOnDeath : MonoBehaviour
	{
		public void OnDisable()
		{
			try
			{
				RpcManager.instance.TriggerWin();
			}
			catch (Exception)
			{
				GameResultData.IsWin = true;
				FindFirstObjectByType<GameOverScreenManager>()?.OpenPanel(true, true);
			}
		}
	}
}