using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Managers;
using Objects.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.Main_Menu.Recollection_Menu
{
	public class RecollectionPanelManager : MonoBehaviour
	{
		[SerializeField] private GameObject recollectionPanel;
		[SerializeField] private List<CharacterDisplayPanel> displayShards;
		[SerializeField] private TextMeshProUGUI gemText;
		[SerializeField] private LootPanel lootPanel;
		private SaveFile _saveFile;

		private void Awake()
		{
			_saveFile = FindObjectOfType<SaveFile>();
		}

		public void OpenRecollectionPanel()
		{
			var bannerCharacters = CharacterListManager.instance
				.GetCharacters().Where(x => x.IsPullable).OrderBy(x => Random.value).Take(5).ToArray();
			for (var i = 0; i < 5; i++)
			{
				displayShards[i].Setup(bannerCharacters[i]);
			}
			
			lootPanel.Setup(_saveFile);
			gemText.text = _saveFile.Gems.ToString();
			recollectionPanel.SetActive(true);
		}
		
		public void CloseRecollectionPanel()
		{
			recollectionPanel.SetActive(false);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				CloseRecollectionPanel();
		}
	}
}