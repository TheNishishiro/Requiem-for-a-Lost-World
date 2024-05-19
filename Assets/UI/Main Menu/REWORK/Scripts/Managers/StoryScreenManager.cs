using System;
using System.Collections.Generic;
using Interfaces;
using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class StoryScreenManager : MonoBehaviour, IStackableWindow
    {
        public static StoryScreenManager instance;
        [Space] 
        [BoxGroup("Chapter")] [SerializeField] private List<StoryChapter> chapterContainers;
        [Space] 
        [BoxGroup("Panels")] [SerializeField] private StoryReader storyReader;
        [Space] 
        [BoxGroup("Animator")] [SerializeField] private Animator animator;

        private void Start()
        {
            if (instance == null)
                instance = this;
        }

        private void Update()
        {
            if (!IsInFocus) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    Close();
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chapter Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Open Chapter"))
                {
                    animator.SetTrigger("Close Chapter");
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open Reader"))
                {
                    animator.SetTrigger("Close Reader");
                }
            }
                
        }

        public void OpenChapter(int id)
        {
            chapterContainers.ForEach(x => x.gameObject.SetActive(false));
            chapterContainers[id - 1].Activate();
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

        public void OpenStoryEntry(TextAsset textFile)
        {
            storyReader.SetText(textFile);
            animator.SetTrigger("Open Reader");
        }
    }
}