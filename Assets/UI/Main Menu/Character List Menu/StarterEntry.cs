using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Main_Menu.Character_List_Menu
{
	public class StarterEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] private Image icon;
		[SerializeField] private TextMeshProUGUI title;
		[SerializeField] private SkillDescriptionPanel skillDescriptionPanel;
		private string _description;

		public void Set(Sprite iconSprite, string titleText, string descriptionText)
		{
			icon.sprite = iconSprite;
			title.text = titleText;
			_description = descriptionText;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			skillDescriptionPanel.Open(icon.sprite, title.text, _description);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			skillDescriptionPanel.Close();
		}
	}
}