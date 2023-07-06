using System;
using System.Collections.Generic;
using Objects.Characters;
using UnityEngine;

namespace UI.Main_Menu.Character_List_Menu
{
	public class RankDisplayPanel : MonoBehaviour
	{
		[SerializeField] private List<EidolonComponent> _eidolonComponents;
		
		public void Open(CharacterData characterData, CharacterRank getRankEnum)
		{
			for (var i = 0; i < 5; i++)
			{
				_eidolonComponents[i].Setup(characterData.Eidolons[i], characterData.ColorTheme, (int)getRankEnum >= i+1);
			}
			gameObject.SetActive(true);
		}
		
		public void Close()
		{
			gameObject.SetActive(false);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				Close();
		}
	}
}