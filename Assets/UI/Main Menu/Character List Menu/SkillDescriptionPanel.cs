using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.Character_List_Menu
{
	public class SkillDescriptionPanel : MonoBehaviour
	{
		[SerializeField] private Image icon;
		[SerializeField] private TextMeshProUGUI title;
		[SerializeField] private TextMeshProUGUI description;
		
		public void Open(Sprite iconSprite, string titleText, string descriptionText)
		{
			icon.sprite = iconSprite;
			title.text = titleText;
			description.text = descriptionText;
			gameObject.SetActive(true);
		}

		public void Close()
		{
			gameObject.SetActive(false);
		}
	}
}