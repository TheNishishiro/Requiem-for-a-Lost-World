using Unity.Netcode;
using UnityEngine;

namespace Managers
{
	public class PauseManager : NetworkBehaviour
	{
		[SerializeField] private CursorManager cursorManager;
		
		public void PauseGame(bool forcePause = false)
		{
			cursorManager.ShowCursor();

			if ((IsHost && NetworkManager.Singleton.ConnectedClients.Count <= 1) || forcePause)
				Time.timeScale = 0;
		}

		public void UnPauseGame()
		{
			cursorManager.HideCursor();
			Time.timeScale = 1;
		}
	}
}