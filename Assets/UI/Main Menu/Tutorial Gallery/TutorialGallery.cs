using System.Collections.Generic;
using DefaultNamespace.Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Main_Menu.Tutorial_Gallery
{
    public class TutorialGallery : MonoBehaviour
    {
        [SerializeField] private List<GameObject> tutorialPages;
        [SerializeField] private ScrollRect scrollRect;
        private SaveFile _saveFile;
        
        public void Start()
        {
            _saveFile = FindObjectOfType<SaveFile>();
			
            if (!_saveFile.IsFirstTutorialCompleted)
                gameObject.SetActive(true);
        }

        public void OnDisable()
        {
            _saveFile.IsFirstTutorialCompleted = true;
            _saveFile.Save();
        }
        
        public void Open(GameObject tutPanel)
        {
            scrollRect.normalizedPosition = new Vector2(0.5f, 1);
            foreach (var childGameObjects in tutorialPages)
            {
                childGameObjects.gameObject.SetActive(false);
            }

            tutPanel.SetActive(true);
        }
    }
}