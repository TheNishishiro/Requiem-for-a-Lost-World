using System;
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
		[SerializeField] private CharacterDisplayPanel backDisplayPanel;
		[SerializeField] private CharacterDisplayPanel sideDisplayPanel;
		[SerializeField] private CharacterDisplayPanel frontDisplayPanel;
		[SerializeField] private TextMeshProUGUI gemText;
		[SerializeField] private TextMeshProUGUI pityCounterText;
		[SerializeField] private LootPanel lootPanel;
		private SaveFile _saveFile;

		private void Awake()
		{
			_saveFile = FindObjectOfType<SaveFile>();
		}

		public void OpenRecollectionPanel()
		{
			var bannerCharacters = CharacterListManager.instance
				.GetCharacters().Where(x => x.IsPullable).OrderBy(x => Random.value).Take(3).ToArray();
			backDisplayPanel.Setup(bannerCharacters[0]);
			sideDisplayPanel.Setup(bannerCharacters[1]);
			frontDisplayPanel.Setup(bannerCharacters[2]);
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