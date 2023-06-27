using Objects.Stage;
using UI.Labels.InGame;
using UnityEngine;
using UnityEngine.Events;

namespace Managers.StageEvents
{
	public class StageTime : MonoBehaviour
	{
		[SerializeField] private GameResultData gameResultData;
		[SerializeField] private UnityEvent<float> onTimeTick;
		public float time;
		private LabelTime _labelTime;

		private void Start()
		{
			_labelTime = FindObjectOfType<LabelTime>();
		}

		private void Update()
		{
			time += Time.deltaTime;
			gameResultData.Time = time;
			_labelTime.UpdateTime(time);
			
			if (Time.frameCount % 60 == 0)
				onTimeTick?.Invoke(time);
		}
	}
}