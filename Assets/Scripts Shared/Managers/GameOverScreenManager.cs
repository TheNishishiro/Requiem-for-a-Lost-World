using TMPro;
using UnityEngine;

namespace Managers
{
	public class GameOverScreenManager : MonoBehaviour
	{
		[SerializeField] private GameObject screenFailed;
		[SerializeField] private GameObject screenVictory;
		[SerializeField] private PauseManager pauseManager;
		
		public void OpenPanel(bool isWin)
		{
			screenFailed.SetActive(!isWin);
			screenVictory.SetActive(isWin);
			
			pauseManager.PauseGame();
		}

		public void QuitGame(bool isWin)
		{
			GameManager.instance.BackToMainMenu(isWin);
		}
	}
}