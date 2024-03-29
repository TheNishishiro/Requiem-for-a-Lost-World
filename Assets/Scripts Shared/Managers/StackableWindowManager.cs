using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Managers
{
    public class StackableWindowManager : MonoBehaviour
    {
        public static StackableWindowManager instance;
        private readonly Stack<IStackableWindow> _windowStack = new();
        private IStackableWindow _currentWindow;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public void OpenWindow(IStackableWindow window, bool isDisablePrevious = true)
        {
            if (isDisablePrevious && _windowStack.TryPeek(out var previousWindow))
            {
                previousWindow.SetActive(false);
            }
                
            _windowStack.Push(window);
            foreach (var stackableWindow in _windowStack)
            {
                stackableWindow.IsInFocus = false;
            }
            window.IsInFocus = true;
            window.SetActive(true);
        }

        public void CloseWindow(IStackableWindow window)
        {
            if (_windowStack.Count > 0 && _windowStack.Peek() == window)
            {
                var currentWindow = _windowStack.Pop();
                currentWindow.IsInFocus = false;
                currentWindow.SetActive(false);
                
                if (_windowStack.Count > 0 && _windowStack.TryPeek(out var previousWindow))
                {
                    previousWindow.IsInFocus = true;
                    previousWindow.SetActive(true);
                }
            }
            else if (!_windowStack.Contains(window))
                window.SetActive(false);
                
        }
    }
}