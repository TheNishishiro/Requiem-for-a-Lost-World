using Objects.Stage;
using UI.Labels.InGame;
using UnityEngine;
using UnityEngine.Events;

namespace Managers.StageEvents
{
	public class StageTime : MonoBehaviour
	{
		[SerializeField] private GameResultData gameResultData;
		public float time;
		private LabelTime _labelTime;
		private float _labelUpdateTimer;
		private float _deltaTimeCache;

		private void Start()
		{
			_labelTime = FindObjectOfType<LabelTime>();
		}

		private void Update()
		{
			_deltaTimeCache = Time.deltaTime;
			
			time += _deltaTimeCache;
			_labelUpdateTimer -= _deltaTimeCache;
			gameResultData.Time = time;

			if (_labelUpdateTimer <= 0)
			{
				_labelTime.UpdateTime(time);
				AchievementManager.instance.OnStageTimeUpdated(time);
				_labelUpdateTimer = 1;
			}
		}
	}
}