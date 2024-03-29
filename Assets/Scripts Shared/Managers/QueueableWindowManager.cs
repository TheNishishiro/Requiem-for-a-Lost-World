using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class QueueableWindowManager : MonoBehaviour
    {
        public static QueueableWindowManager instance;
        [SerializeField] private PauseManager pauseManager;
        private readonly Stack<IQueueableWindow> _windowQueue = new();
        private IQueueableWindow _currentWindow;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public bool IsQueueEmpty()
        {
            return _windowQueue.Count == 0;
        }

        public void QueueWindow(IQueueableWindow window)
        {
            PauseGame();

            if (_currentWindow == null)
            {
                _currentWindow = window;
                _currentWindow.Open();
                return;
            }

            _windowQueue.Push(window);
        }

        public void QueueUniqueWindow(IQueueableWindow window)
        {
            if (!_windowQueue.Contains(window))
                QueueWindow(window);
        }

        public void DeQueueWindow()
        {
            if (_currentWindow == null && _windowQueue.Count == 0)
            {
                ResumeGame();
                return;
            }

            if (_windowQueue.Count == 0 && _currentWindow != null)
            {
                _currentWindow.Close();
                ResumeGame();
                _currentWindow = null;
                return;
            }

            _currentWindow?.Close();
            _currentWindow = _windowQueue.Pop();
            _currentWindow.Open();
        }

        private void PauseGame()
        {
            if (pauseManager != null)
                pauseManager.PauseGame();
        }

        private void ResumeGame()
        {
            if (pauseManager != null)
                pauseManager.UnPauseGame();
        }
    }
}