using System.Collections;
using TMPro;
using UnityEngine;

namespace CarouselUI
{
    public class CarouselUIElement : MonoBehaviour
    {
        [Header("Carousel Members")]
        [SerializeField] private string[] _options;
        [SerializeField] private TextMeshProUGUI _textLabel;
        [SerializeField] private GameObject _nextButton;
        [SerializeField] private GameObject _prevButton;

        [Header("Settings")]
        [SerializeField] private float _resetDuration = 0.1f;
        [SerializeField] private bool _doesNotCycleBack = false;

        private int _currentIndex = 0;

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { _currentIndex = value; }
        }

        public delegate void InputDetected();
        public event InputDetected InputEvent = delegate { };

        private bool _isProcessing = false; //HERE TO DELAY REFIRES
        private WaitForSeconds _resetDelay; //WORKS WITH DELAY COROUTINE

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_options.Length == 0 || _options == null) //ERROR IF THE OPTIONS ARRAY IS EMPTY
            {
                Debug.LogError($"Carousel UI at {this.gameObject.name} has incomplete options array. Please fix.");

                return;
            }

            _resetDelay = new WaitForSeconds(_resetDuration);

            UpdateUI();
        }

        private void UpdateUI()
        {
            _textLabel.text = _options[_currentIndex];

            if (_doesNotCycleBack && _currentIndex == _options.Length - 1)
            {
                _nextButton.SetActive(false);
            }
            else
            {
                _nextButton.SetActive(true);
            }

            if (_doesNotCycleBack && _currentIndex == 0)
            {
                _prevButton.SetActive(false);
            }
            else
            {
                _prevButton.SetActive(true);
            }

        }

        /// <summary>
        /// Prevents further refires until duration ends.
        /// </summary>
        /// <returns></returns>
        private IEnumerator LockoutDelay()
        {
            _isProcessing = true; //PREVENTS BUTTON MASHING

            yield return _resetDelay;

            _isProcessing = false;

            yield break;
        }

        //METHOD ACCESSED BY NEXT BUTTON
        public void PressNext()
        {
            if (_isProcessing)
            {
                return;
            }

            if (_doesNotCycleBack && _currentIndex == _options.Length - 1)
            {
                return;
            }

            StartCoroutine(LockoutDelay());

            if (_currentIndex < (_options.Length - 1))
            {
                _currentIndex += 1;

                UpdateUI();
            }
            else
            {
                _currentIndex = 0;

                UpdateUI();
            }

            InputEvent?.Invoke();
        }

        //METHOD ACCESSED BY PREVIOUS BUTTON
        public void PressPrevious()
        {
            if (_isProcessing)
            {
                return;
            }

            if (_doesNotCycleBack && _currentIndex == 0)
            {
                return;
            }

            StartCoroutine(LockoutDelay());

            if (_currentIndex > 0)
            {
                _currentIndex -= 1;

                UpdateUI();
            }
            else
            {
                _currentIndex = (_options.Length - 1);

                UpdateUI();
            }

            InputEvent?.Invoke();
        }

        /// <summary>
        /// Used by an associated processor to update the index of this carousel.
        /// </summary>
        /// <param name="input"></param>
        public void UpdateIndex(int input)
        {
            _currentIndex = input;
            UpdateUI();
        }
    }
}
