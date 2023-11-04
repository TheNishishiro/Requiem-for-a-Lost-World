using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace UI.Main_Menu.TitleScreen
{
	public class TitleScreen : MonoBehaviour
    {
        public TutorialManager tutorialManager;
        public CanvasGroup titleScreen; // Assumes an Image component. If you're using a CanvasGroup, change this to CanvasGroup.
        public Animator titleScreenAnimator;
        public GameObject mainMenu; // The animator of your title screen.
        public GameObject versionText;
        public float fadeDuration = 1.0f; // Duration of the fade effect in seconds.

        private void Update()
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                titleScreenAnimator.Update(float.PositiveInfinity);
                mainMenu.SetActive(true);
                versionText.SetActive(false);
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