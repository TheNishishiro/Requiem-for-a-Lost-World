using System;
using Managers;
using UnityEngine;

namespace Objects.Enemies
{
	public class EnemyWinOnDeath : MonoBehaviour
	{
		public void OnDisable()
		{
			FindFirstObjectByType<GameOverScreenManager>()?.OpenPanel(true);
		}
	}
}