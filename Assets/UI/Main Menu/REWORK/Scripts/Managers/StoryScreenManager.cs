using System.Collections.Generic;
using Interfaces;
using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class StoryScreenManager : MonoBehaviour, IStackableWindow
    {
        [Space] 
        [BoxGroup("Chapter")] [SerializeField] private List<GameObject> chapterContainers;
        [Space] 
        [BoxGroup("Animator")] [SerializeField] private Animator animator;

        public void OpenChapter(int id)
        {
            chapterContainers.ForEach(x => x.SetActive(false));
            chapterContainers[id - 1].SetActive(true);
            animator.SetTrigger("Open Chapter");
        }
        
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