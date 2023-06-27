using System;
using Managers;
using UnityEngine;

namespace Objects.Enemies
{
	public class EnemyWinOnDeath : MonoBehaviour
	{
		public void OnDestroy()
		{
			FindObjectOfType<GameOverScreenManager>()?.OpenPanel(true);
		}
	}
}