using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

namespace UI.Main_Menu.REWORK.Scripts
{
    public class StoryReader : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textContent;
        private string _currentText;
        private bool _skipRequest;
        private const float TextSpeed = 0.05f;
        private Coroutine _readingRoutine;
        private readonly StringBuilder _stringBuilder = new();
        
        public void SetText(TextAsset textFile)
        {
            _currentText = textFile.text;
            _skipRequest = false;
        }

        public void OnEnable()
        {
            if (_readingRoutine != null)
            {
                StopCoroutine(_readingRoutine);
            }
            _readingRoutine = StartCoroutine(ReadText());
        }


        private IEnumerator ReadText()
        {
            _stringBuilder.Clear();

            foreach (var character in _currentText.ToCharArray())
            {
                if (_skipRequest)
                {
                    _stringBuilder.Clear();
                    _stringBuilder.Append(_currentText);
                    break;
                }

                _stringBuilder.Append(character);
                textContent.text = _stringBuilder.ToString();
                yield return new WaitForSeconds(TextSpeed);
            }

            _skipRequest = false;
        }
        
        public void SkipReading()
        {
            _skipRequest = true;
        }
    }
}