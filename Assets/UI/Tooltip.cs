using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
	public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public string message;
		
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (string.IsNullOrWhiteSpace(message))
				return;
			
			TooltipManager.instance.ShowTooltip(message);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			TooltipManager.instance.HideTooltip();
		}
	}
}