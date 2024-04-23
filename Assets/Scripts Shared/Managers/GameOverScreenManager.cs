using TMPro;
using UnityEngine;

namespace Managers
{
	public class GameOverScreenManager : MonoBehaviour
	{
		[SerializeField] private GameObject screenFailed;
		[SerializeField] private GameObject screenVictory;
		[SerializeField] private PauseManager pauseManager;
		
		public void OpenPanel(bool isWin, bool forcePause = false)
		{
			screenFailed.SetActive(!isWin);
			screenVictory.SetActive(isWin);
			
			pauseManager.PauseGame(forcePause);
		}

		public void QuitGame(bool isWin)
		{
			GameManager.instance.BackToMainMenu(isWin);
		}
	}
}