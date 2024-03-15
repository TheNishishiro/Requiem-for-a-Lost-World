using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class WindowManager : MonoBehaviour
    {
        public static WindowManager instance;
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
            pauseManager.PauseGame();
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
                pauseManager.UnPauseGame();
                return;
            }

            if (_windowQueue.Count == 0 && _currentWindow != null)
            {
                _currentWindow.Close();
                pauseManager.UnPauseGame();
                _currentWindow = null;
                return;
            }

            _currentWindow?.Close();
            _currentWindow = _windowQueue.Pop();
            _currentWindow.Open();
        }
    }
}