using DefaultNamespace.Data;
using UnityEngine;

namespace Managers
{
	public class TutorialManager : MonoBehaviour
	{
		public GameObject StartTutorial;
		private SaveFile _saveFile;
		
		public void Start()
		{
			_saveFile = FindObjectOfType<SaveFile>();
			
			if (!_saveFile.IsFirstTutorialCompleted)
				StartTutorial.SetActive(true);
		}

		public void MarkFirstTutorialComplete()
		{
			_saveFile.IsFirstTutorialCompleted = true;
			_saveFile.Save();
		}
	}
}