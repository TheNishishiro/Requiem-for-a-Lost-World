using System;
using UnityEngine;

namespace UI.Main_Menu.Level_Selection
{
	public class StageSelectionPanel : MonoBehaviour
	{
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				Close();
		}
		
		public void Toggle()
		{
			if (gameObject.activeSelf)
				Close();
			else
				Open();
		}

		public void Close()
		{
			gameObject.SetActive(false);
		}

		public void Open()
		{
			gameObject.SetActive(true);
		}
	}
}