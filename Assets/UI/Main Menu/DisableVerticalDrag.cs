using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Main_Menu
{
    public class DisableScrollRectOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private ScrollRect scrollRect;
        private bool wasHorizontalDrag;

        void Start()
        {
            scrollRect = GetComponent<ScrollRect>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y))
            {
                scrollRect.enabled = false;
                wasHorizontalDrag = true;
            }
        }

        public void OnDrag(PointerEventData eventData) {}

        public void OnEndDrag(PointerEventData eventData)
        {
            if (wasHorizontalDrag)
            {
                scrollRect.enabled = true;
                wasHorizontalDrag = false;
            }
        }
    }
}