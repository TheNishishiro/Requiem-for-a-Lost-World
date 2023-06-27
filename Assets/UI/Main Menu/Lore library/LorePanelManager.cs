using System.Collections;
using System.Collections.Generic;
using Managers;
using UI.Main_Menu.Lore_library;
using UnityEngine;

public class LorePanelManager : MonoBehaviour
{
	[SerializeField] private GameObject panel;
	[SerializeField] private LoreLibraryContainer container;
	
	public void Open()
	{
		panel.SetActive(true);
		
		var activeCharacter = CharacterListManager.instance.GetActiveCharacter();
		var activeCharacterSaveData = CharacterListManager.instance.GetActiveCharacterData();
		container.Refresh(activeCharacter, activeCharacterSaveData);
	}
	
	public void Close()
	{
		panel.SetActive(false);
		container.Clear();
	}
}
