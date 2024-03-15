using Managers;
using Objects.Players.Scripts;
using Objects.Stage;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityTemplateProjects;

namespace UI.Labels.InGame.PauseMenu
{
	public class ExitButton : MonoBehaviour
	{
		public void BackToMainMenu(bool isWin)
		{
			GameManager.instance.BackToMainMenu(isWin);
		}
	}
}