using Objects.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Main_Menu.Lore_library
{
	public class LoreChapterEntryPanel : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] private TextMeshProUGUI title;
		private CharacterLoreEntry _loreEntry;
		
		public void Setup(CharacterLoreEntry loreEntry)
		{
			_loreEntry = loreEntry;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			var loreEntryContainer = FindObjectOfType<LoreEntryPanel>(true);
			loreEntryContainer.Open(_loreEntry);
		}
	}
}