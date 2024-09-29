using System;
using DefaultNamespace.Data.Locale;
using Events.Handlers;
using Events.Scripts;
using Managers;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.Locale
{
    public class TextMeshProTranslator : MonoBehaviour, ITranslationChangedHandler
    {
        private TextMeshProUGUI _textComponent;
        
        private void Start()
        {
            _textComponent = GetComponent<TextMeshProUGUI>();
        }
        
        public void OnTranslationChanged()
        {
            _textComponent.text = _textComponent.text.Translate();
        }

        private void OnEnable()
        {
            TranslationChangedEvent.Register(this);
        }

        private void OnDisable()
        {
            TranslationChangedEvent.Unregister(this);
        }
    }
}