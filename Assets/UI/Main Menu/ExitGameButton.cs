using Managers;
using UnityEngine;

namespace UI.Main_Menu
{
	public class ExitGameButton : MonoBehaviour
	{
		public void QuitApplication()
		{
			var discordManager = FindObjectOfType<DiscordManager>();
			if (discordManager != null)
				discordManager.ClearActivity();
			Application.Quit();
		}
	}
}