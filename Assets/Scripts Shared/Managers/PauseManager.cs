using Unity.Netcode;
using UnityEngine;

namespace Managers
{
	public class PauseManager : NetworkBehaviour
	{
		[SerializeField] private CursorManager cursorManager;
		[SerializeField] private BlurManager blurManager;
		
		public void PauseGame()
		{
			cursorManager.ShowCursor();
			blurManager.Blur();

			if (IsHost && NetworkManager.Singleton.ConnectedClients.Count <= 1)
				Time.timeScale = 0;
		}

		public void UnPauseGame()
		{
			cursorManager.HideCursor();
			blurManager.DeBlur();
			Time.timeScale = 1;
		}
	}
}