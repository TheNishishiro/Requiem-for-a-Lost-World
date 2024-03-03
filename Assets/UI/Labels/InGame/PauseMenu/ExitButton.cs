using Managers;
using Objects.Players.Scripts;
using Objects.Stage;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityTemplateProjects;

namespace UI.Labels.InGame.PauseMenu
{
	public class ExitButton : NetworkBehaviour
	{
		public void BackToMainMenu(bool isWin)
		{
			GameManager.instance.BackToMainMenu(isWin);
		}
	}
}