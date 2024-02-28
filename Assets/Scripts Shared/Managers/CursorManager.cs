using System;
using Objects.Stage;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
	public class CursorManager : MonoBehaviour
	{
		[SerializeField] private StarterAssetsInputs starterAssetsInputs;

		private void Start()
		{
			if (!GameData.GetPlayerCharacterData().PickWeaponOnStart)
				HideCursor();
			if (Application.platform == RuntimePlatform.Android)
				ShowCursor();
		
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.LeftControl))
			{
				ShowCursor();
			}

			if (Input.GetKeyUp(KeyCode.LeftControl))
			{
				HideCursor();
			}
		}

		public void ShowCursor()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			starterAssetsInputs.cursorInputForLook = false;
			starterAssetsInputs.cursorLocked = false;
			starterAssetsInputs.LookInput(Vector2.zero);
		}

		public void HideCursor()
		{
			if (Application.platform == RuntimePlatform.Android) return;
			
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			starterAssetsInputs.cursorInputForLook = true;
			starterAssetsInputs.cursorLocked = true;
		}
	}
}