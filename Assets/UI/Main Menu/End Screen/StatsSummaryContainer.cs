using System.Collections.Generic;
using System.Linq;
using Objects.Stage;
using UnityEngine;

namespace UI.Main_Menu.End_Screen
{
	public class StatsSummaryContainer : MonoBehaviour
	{
		[SerializeField] private PanelStatsSummary panelStatsSummaryPrefab;
		private List<GameObject> _statsSummaries;

		public void Setup()
		{
			_statsSummaries ??= new List<GameObject>();
			var entryId = 0;
			var delay = 0f;
			foreach (var statSummary in GameResultData.GetStatsSummary())
			{
				var panelStatsSummary = Instantiate(panelStatsSummaryPrefab, transform);
				panelStatsSummary.Setup(statSummary, entryId++ % 2 != 0, delay += 0.1f);
				_statsSummaries.Add(panelStatsSummary.gameObject);
			}
		}

		public void Clear()
		{
			_statsSummaries ??= new List<GameObject>();
			foreach (var panelDamageSummary in _statsSummaries)
			{
				Destroy(panelDamageSummary);
			}
			_statsSummaries.Clear();
		}
	}
}