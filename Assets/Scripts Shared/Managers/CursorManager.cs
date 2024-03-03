using System;
using System.Collections;
using Objects.Stage;
using StarterAssets;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
	public class CursorManager : MonoBehaviour
	{
		private StarterAssetsInputs _starterAssetsInputs;

		public void Setup(StarterAssetsInputs starterAssetsInputs)
		{
			_starterAssetsInputs = starterAssetsInputs;
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
			if (_starterAssetsInputs == null) return;
			
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			_starterAssetsInputs.cursorInputForLook = false;
			_starterAssetsInputs.cursorLocked = false;
			_starterAssetsInputs.LookInput(Vector2.zero);
		}

		public void HideCursor()
		{
			if (Application.platform == RuntimePlatform.Android) return;
			if (_starterAssetsInputs == null) return;
			
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			_starterAssetsInputs.cursorInputForLook = true;
			_starterAssetsInputs.cursorLocked = true;
		}
	}
}