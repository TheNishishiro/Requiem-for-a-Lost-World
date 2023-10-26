using DefaultNamespace.Data;
using UI.Main_Menu.Tutorial_Gallery;
using UnityEngine;

namespace Managers
{
	public class TutorialManager : MonoBehaviour
	{
		public TutorialGallery tutorialPanel;
		public GameObject firstTutorial;
		private SaveFile _saveFile;
		
		public void DisplayFirst()
		{
			_saveFile = FindObjectOfType<SaveFile>();

			if (!_saveFile.IsFirstTutorialCompleted)
			{
				tutorialPanel.gameObject.SetActive(true);
				tutorialPanel.Open(firstTutorial);
				MarkFirstTutorialComplete();
			}
		}

		public void MarkFirstTutorialComplete()
		{
			_saveFile.IsFirstTutorialCompleted = true;
		}
	}
}