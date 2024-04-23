using System;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Objects.Stage;
using UI.Labels.InGame.Minimap;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Labels.InGame
{
	public class UiManager : MonoBehaviour
	{
		[SerializeField] private GameObject uiControlsCanvas;
		
		public void Awake()
		{
			uiControlsCanvas.SetActive(Touchscreen.current != null);
		}
	}
}