using DefaultNamespace.Data;
using TMPro;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class StoryEntry : MonoBehaviour
    {
        [SerializeField] private TextAsset textFile;
        [SerializeField] private float pointsRequired;
        [SerializeField] private GameObject lockGameObject;
        [SerializeField] private TextMeshProUGUI unlockPercentage;

        public void Refresh()
        {
            lockGameObject.SetActive(SaveFile.Instance.StoryPoints < pointsRequired);
            unlockPercentage.text = $"{(SaveFile.Instance.StoryPoints / pointsRequired)*100f:N0}%";
        }
        
        public void Open()
        {
            StoryScreenManager.instance.OpenStoryEntry(textFile);
        }
    }
}