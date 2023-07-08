using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Main_Menu.Character_List_Menu
{
	public class StatsMenuEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] private TextMeshProUGUI statName;
		[SerializeField] private TextMeshProUGUI statValue;
		private TextMeshProUGUI _descriptionField;
		private string _description;
		private bool _showDescription;
		
		public void Setup(string name, string value, TextMeshProUGUI descriptionField, bool showDescription, string description)
		{
			_descriptionField = descriptionField;
			_showDescription = showDescription;
			_description = description;
			Setup(name, value);
		}		
		
		public void Setup(string name, string value)
		{
			statName.text = name;
			statValue.text = value;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!_showDescription) return;
			
			_descriptionField.gameObject.SetActive(true);
			_descriptionField.text = _description;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (!_showDescription) return;
			
			_descriptionField.gameObject.SetActive(false);
		}
	}
}