using UnityEngine;

namespace Managers
{
	public class PauseManager : MonoBehaviour
	{
		[SerializeField] private CursorManager cursorManager;
		[SerializeField] private BlurManager blurManager;
		
		public void PauseGame()
		{
			cursorManager.ShowCursor();
			blurManager.Blur();
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