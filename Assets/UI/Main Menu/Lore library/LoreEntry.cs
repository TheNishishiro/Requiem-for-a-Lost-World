using Objects.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu.Lore_library
{
	public class LoreEntry : MonoBehaviour
	{
		[SerializeField] Image icon;
		[SerializeField] TextMeshProUGUI title;
		private CharacterLoreEntry _loreEntry;
		
		public void SetEntry(CharacterLoreEntry loreEntry)
		{
			_loreEntry = loreEntry;
			title.text = loreEntry.Title;
		}
		
		public void Open()
		{
			GetComponentInParent<LoreLibraryContainer>().OpenEntryPanel(_loreEntry);
		}
	}
}