﻿using System;
using System.Linq;
using UnityEngine;

namespace UI.Main_Menu
{
	public class ShowPanelButton : MonoBehaviour
	{
		[SerializeField] public GameObject panelToShow;
		
		public void ShowPanel()
		{
			panelToShow.SetActive(true);
		}
		
		public void HidePanel()
		{
			panelToShow.SetActive(false);
		}
		
		public void TogglePanel()
		{
			panelToShow.SetActive(!panelToShow.activeSelf);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				HidePanel();
		}
	}
}