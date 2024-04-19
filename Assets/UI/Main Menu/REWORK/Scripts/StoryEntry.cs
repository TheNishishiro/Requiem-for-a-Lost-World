using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class StoryEntry : MonoBehaviour
    {
        [SerializeField] private TextAsset textFile;

        public void Open()
        {
            StoryScreenManager.instance.OpenStoryEntry(textFile);
        }
    }
}