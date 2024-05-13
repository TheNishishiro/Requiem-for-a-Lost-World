using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Shared.Utilities
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LinkHandlerForTMPText : MonoBehaviour, IPointerClickHandler
    {
        private TextMeshProUGUI _textMeshProUGUI;
        private Canvas _canvasToCheck;
        private Camera _cameraToUse;
        
        private void Awake()
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            _canvasToCheck = GetComponentInParent<Canvas>();
            _cameraToUse = _canvasToCheck.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvasToCheck.worldCamera;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var mousePosition = new Vector3(eventData.position.x, eventData.position.y, 0);
            var linkTaggedText = TMP_TextUtilities.FindIntersectingLink(_textMeshProUGUI, mousePosition, _cameraToUse);
            if (linkTaggedText == -1) return;
            
            var linkInfo = _textMeshProUGUI.textInfo.linkInfo[linkTaggedText];
            Application.OpenURL(linkInfo.GetLinkID());
        }
    }
}