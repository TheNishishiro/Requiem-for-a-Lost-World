using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UI.Main_Menu.REWORK.Scripts;
using UnityEngine;

namespace UI.Main_Menu.TitleScreen
{
	public class TitleScreen : MonoBehaviour
    {
        [SerializeField] private MainMenuManager mainMenuManager;
        public TutorialManager tutorialManager;
        public CanvasGroup titleScreen;
        public float fadeDuration = 1.0f;
        private bool _canClose;
        
        public void MarkAsCanClose()
        {
            _canClose = true;
        }
        
        private void Update()
        {
            if (!_canClose) return;
            
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                mainMenuManager.gameObject.SetActive(true);
                StartCoroutine(FadeOutTitleScreen());
                tutorialManager.DisplayFirst();
                AudioManager.instance.PlayButtonConfirmClick();
            }
        }

        IEnumerator FadeOutTitleScreen()
        {
            var elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                var alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

                titleScreen.alpha = alpha;
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}