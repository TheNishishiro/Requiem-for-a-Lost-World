using System;
using TMPro;
using UnityEngine;

namespace UI
{
	public class TooltipManager : MonoBehaviour
	{
		public static TooltipManager instance;
		[SerializeField] private TextMeshProUGUI tooltipText;
		public Camera camera;

		private void Start()
		{
			HideTooltip();
			if (instance == null)
				instance = this;
		}

		public void ShowTooltip(string text)
		{
			gameObject.SetActive(true);
			tooltipText.text = text;
		}

		public void HideTooltip()
		{
			gameObject.SetActive(false);
			tooltipText.text = string.Empty;
		}

		private void Update()
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = -camera.transform.position.z;
            var worldPosition= camera.ScreenToWorldPoint(mousePosition);
        
            var offset = new Vector3(10,10,0); // Change this to your desired offset
            
            transform.position = worldPosition + offset;
        }
	}
}