using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Labels.InGame.PauseMenu
{
	public class ExitButton : MonoBehaviour
	{
		[SerializeField] private GameResultData gameResultData;
		
		public void BackToMainMenu(bool isWin)
		{
			gameResultData.IsGameEnd = true;
			gameResultData.IsWin = isWin;
			gameResultData.Level = FindObjectOfType<LevelComponent>()?.GetLevel() ?? 0;
			
			SceneManager.LoadScene("Scenes/Main Menu");
		}
	}
}