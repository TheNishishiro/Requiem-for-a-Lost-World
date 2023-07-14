using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace UI.Shared.Animations
{
    public class SlideAnimator : MonoBehaviour
    {
        public enum SlideFrom { Left, Right }

        public SlideFrom slideSource = SlideFrom.Left;
        public float delay = 0f;
        public float duration = 1f;
        public UnityEvent onShow;
        public UnityEvent onHide;

        private RectTransform rectTransform;
        private Vector2 endPosition;
        private Vector2 startPosition;
        private bool disableOnHidden = true;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            endPosition = rectTransform.anchoredPosition;
            ResetPosition();
        }

        private void ResetPosition()
        {
            rectTransform = GetComponent<RectTransform>();
            startPosition = slideSource == SlideFrom.Left 
                ? new Vector2(-rectTransform.rect.width, rectTransform.anchoredPosition.y)
                : new Vector2(rectTransform.rect.width, rectTransform.anchoredPosition.y);
        }

        public void ShowPanel()
        {
            ResetPosition();
            gameObject.SetActive(true);
            StartCoroutine(Slide(startPosition, endPosition, false));
        }

        public void HidePanel()
        {
            if (!gameObject.activeSelf)
                return;
            StartCoroutine(Slide(endPosition, startPosition, true));
        }

        private IEnumerator Slide(Vector3 positionFrom, Vector3 positionTo, bool isHide)
        {
            yield return new WaitForSeconds(delay);
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                rectTransform.anchoredPosition = Vector3.Lerp(positionFrom, positionTo, t / duration);
                yield return null;
            }
            rectTransform.anchoredPosition = positionTo;
            
            if (isHide)
                onHide?.Invoke();
            else
                onShow?.Invoke();
            
            if (isHide && disableOnHidden)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
