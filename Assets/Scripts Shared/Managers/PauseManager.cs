using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Data.Game;
using Unity.Netcode;
using UnityEngine;

namespace Managers
{
	public class PauseManager : NetworkBehaviour
	{
		public static PauseManager instance;
		private readonly Dictionary<GamePauseStates, int> _gamePauseStateCounter = new ();

		public void Start()
		{
			if (instance == null)
			{
				instance = this;
				foreach (var pauseState in Utilities.EnumToEnumerable<GamePauseStates>())
				{
					_gamePauseStateCounter.TryAdd(pauseState, 0);
				}
			}
		}
		
		public void PauseGame(bool awaitPlayerVotes = false)
		{
			if (IsHost)
			{
				RpcManager.instance.PauseRpc(awaitPlayerVotes);
			}
			else
			{
				CursorManager.instance.ShowCursor();
			}
		}

		public void UnPauseGame(bool awaitPlayerVotes = false)
		{
			if (IsHost)
			{
				if (awaitPlayerVotes)
					StartCoroutine(AwaitPlayersReadyForUnPause());
				else
					RpcManager.instance.UnPauseRpc();
			}
			else
			{
				CursorManager.instance.HideCursor();
			}
		}

		private IEnumerator AwaitPlayersReadyForUnPause()
		{
			var players = NetworkManager.Singleton.ConnectedClients
				.Select(x => x.Value.PlayerObject.GetComponent<MultiplayerPlayer>())
				.ToList();
			do
			{
				yield return new WaitForSecondsRealtime(0.1f);
			} while (players.Count > 1 && players.Any(x => !x.isVoteUnpause.Value));

			RpcManager.instance.UnPauseRpc();
		}

		public void AddPauseState(GamePauseStates pauseState)
		{
			_gamePauseStateCounter[pauseState]++;
		}

		public void RemovePauseState(GamePauseStates pauseState)
		{
			_gamePauseStateCounter[pauseState]--;
			if (_gamePauseStateCounter[pauseState] < 0)
				_gamePauseStateCounter[pauseState] = 0;
		}

		public bool IsPauseStateSet(GamePauseStates pauseState)
		{
			return _gamePauseStateCounter[pauseState] > 0;
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
			foreach (GamePauseStates state in Enum.GetValues(typeof(GamePauseStates)))
			{
				RemovePauseState(state);
			}
		}
	}
}