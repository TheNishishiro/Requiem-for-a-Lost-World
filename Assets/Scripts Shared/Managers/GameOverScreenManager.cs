﻿using TMPro;
using UnityEngine;

namespace Managers
{
	public class GameOverScreenManager : MonoBehaviour
	{
		[SerializeField] private GameObject gameOverScreen;
		[SerializeField] private PauseManager pauseManager;
		[SerializeField] private TextMeshProUGUI gameOverText;
		[SerializeField] private GameObject victoryButton;
		[SerializeField] private GameObject defeatButton;
		
		public void OpenPanel(bool isWin, bool forcePause = false)
		{
			gameOverText.text = isWin ? "Victory!" : "Retreat!";
			victoryButton.SetActive(isWin);
			defeatButton.SetActive(!isWin);
			
			pauseManager.PauseGame(forcePause);
			gameOverScreen.SetActive(true);
		}
	}
}