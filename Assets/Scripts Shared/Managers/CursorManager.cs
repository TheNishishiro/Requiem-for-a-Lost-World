using System;
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
			HideCursor();
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
		}

		public void HideCursor()
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			starterAssetsInputs.cursorInputForLook = true;
			starterAssetsInputs.cursorLocked = true;
		}
	}
}