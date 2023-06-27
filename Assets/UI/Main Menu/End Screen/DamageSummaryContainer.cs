using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Stage;
using TMPro;
using UnityEngine;

namespace UI.Main_Menu.End_Screen
{
	public class DamageSummaryContainer : MonoBehaviour
	{
		[SerializeField] private PanelDamageSummary panelDamageSummaryPrefab;
		private List<GameObject> _panelDamageSummaries;

		public void Setup(GameResultData gameResultData)
		{
			_panelDamageSummaries ??= new List<GameObject>();
			if (!gameResultData.ItemDamage.Any())
				return;
			
			var maxDamage = gameResultData.ItemDamage.Values.Max();
			foreach (var damageSummaryEntry in gameResultData.ItemDamage.OrderByDescending(x => x.Value))
			{
				var panelDamageSummary = Instantiate(panelDamageSummaryPrefab, transform);
				panelDamageSummary.Setup(new DamageSummaryEntry()
				{
					Icon = damageSummaryEntry.Key.Icon,
					RawDamage = damageSummaryEntry.Value,
					MaxDamage = maxDamage,
					Name = damageSummaryEntry.Key.Name
				});
				_panelDamageSummaries.Add(panelDamageSummary.gameObject);
			}
		}

		public void Clear()
		{
			_panelDamageSummaries ??= new List<GameObject>();
			foreach (var panelDamageSummary in _panelDamageSummaries)
			{
				Destroy(panelDamageSummary);
			}
			_panelDamageSummaries.Clear();
		}
	}
}