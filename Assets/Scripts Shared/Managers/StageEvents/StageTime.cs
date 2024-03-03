using Objects.Stage;
using UI.Labels.InGame;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Managers.StageEvents
{
	public class StageTime : NetworkBehaviour
	{
		[SerializeField] private GameResultData gameResultData;
		public NetworkVariable<float> time = new ();
		private LabelTime _labelTime;
		private float _labelUpdateTimer;
		private float _deltaTimeCache;
		private bool _isInit;

		public override void OnNetworkSpawn()
		{
			_labelTime = FindFirstObjectByType<LabelTime>();
			_isInit = true;
		}

		private void Update()
		{
			if (!_isInit) return;
			
			_deltaTimeCache = Time.deltaTime;
			if (IsHost)
				time.Value += _deltaTimeCache;
			_labelUpdateTimer -= _deltaTimeCache;
			gameResultData.Time = time.Value;

			if (_labelUpdateTimer <= 0) 
			{
				_labelTime.UpdateTime(time.Value);
				AchievementManager.instance.OnStageTimeUpdated(time.Value);
				_labelUpdateTimer = 1;
			}
		}
	}
}