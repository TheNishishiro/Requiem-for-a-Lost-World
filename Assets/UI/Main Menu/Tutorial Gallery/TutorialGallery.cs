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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                gameObject.SetActive(false);
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