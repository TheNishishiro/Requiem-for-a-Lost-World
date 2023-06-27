using Managers;
using Objects.Stage;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Main_Menu
{
	public class PlayButton : MonoBehaviour
	{
		public void StartGame()
		{
			SceneManager.LoadScene("Scenes/Main Level", LoadSceneMode.Single);
			SceneManager.LoadScene("Scenes/Essential", LoadSceneMode.Additive);
		}
	}
}