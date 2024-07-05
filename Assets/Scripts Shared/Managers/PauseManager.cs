using System;
using DefaultNamespace.Data.Game;
using Unity.Netcode;
using UnityEngine;

namespace Managers
{
	public class PauseManager : NetworkBehaviour
	{
		public static PauseManager instance;
		[SerializeField] private CursorManager cursorManager;
		private GamePauseStates _gamePauseStates;

		public void Start()
		{
			if (instance == null)
				instance = this;
		}
		
		public void PauseGame(bool forcePause = false)
		{
			cursorManager.ShowCursor();

			if ((IsHost && NetworkManager.Singleton.ConnectedClients.Count <= 1) || forcePause)
				Time.timeScale = 0;
		}

		public void UnPauseGame()
		{
			cursorManager.HideCursor();
			Time.timeScale = 1;
		}

		public void AddPauseState(GamePauseStates pauseState)
		{
			_gamePauseStates |= pauseState;
		}

		public void RemovePauseState(GamePauseStates pauseState)
		{
			_gamePauseStates &= ~pauseState;
		}

		public bool IsPauseStateSet(GamePauseStates pauseState)
		{
			return (_gamePauseStates & pauseState) != 0;
		}

		public void SetFullPause()
		{
			foreach (GamePauseStates state in Enum.GetValues(typeof(GamePauseStates)))
			{
				AddPauseState(state);
			}
		}

		public void ClearFullPause()
		{
			_gamePauseStates = 0;
		}
	}
}