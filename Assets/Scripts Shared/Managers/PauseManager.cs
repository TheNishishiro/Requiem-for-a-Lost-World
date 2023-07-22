using UnityEngine;

namespace Managers
{
	public class PauseManager : MonoBehaviour
	{
		public void PauseGame()
		{
			Time.timeScale = 0;
		}

		public void UnPauseGame()
		{
			Time.timeScale = 1;
		}
	}
}