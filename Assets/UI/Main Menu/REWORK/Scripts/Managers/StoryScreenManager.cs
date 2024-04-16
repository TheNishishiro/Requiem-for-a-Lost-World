using Interfaces;
using Managers;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class StoryScreenManager : MonoBehaviour, IStackableWindow
    {
        public void Open()
        {
            StackableWindowManager.instance.OpenWindow(this);
        }

        public void Close()
        {
            StackableWindowManager.instance.CloseWindow(this);
        }
        
        public bool IsInFocus { get; set; }
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}