using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class StoryChapter : MonoBehaviour
    {
        public void Activate()
        {
            gameObject.SetActive(true);
            foreach (var storyEntry in GetComponentsInChildren<StoryEntry>())
            {
                storyEntry.Refresh();
            }
        }
    }
}