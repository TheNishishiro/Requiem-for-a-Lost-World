using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace UI.Shared
{
    public class FancyNotificationDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Animator animator;
        [SerializeField] private Color color;
        [SerializeField] private List<UIPrimitiveBase> uiElements;
        [SerializeField] private Image descriptionBackground;
        [SerializeField] private ParticleSystem particleGlow;
        private float _displayTime;
        
        public void Display(string text, string title, Color? newColor =  null)
        {
            _displayTime = 5f;
            var themeColor = newColor ?? color;
            var particleMainModule = particleGlow.main;
            particleMainModule.startColor = new Color(themeColor.r, themeColor.g, themeColor.b, descriptionBackground.color.a);
            gameObject.SetActive(true);
            mainText.text = text;
            titleText.text = title;
            descriptionBackground.color = new Color(themeColor.r, themeColor.g, themeColor.b, descriptionBackground.color.a);
            foreach (var uiElement in uiElements)
            {
                uiElement.color = new Color(themeColor.r, themeColor.g, themeColor.b, uiElement.color.a);
            }
        }

        private void Update()
        {
            if (_displayTime > 0)
            {
                _displayTime -= Time.deltaTime;
                return;
            }
			
            animator.SetTrigger("Hide");
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}