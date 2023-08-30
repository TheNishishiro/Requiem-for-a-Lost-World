using System.Collections.Generic;
using System.Collections.ObjectModel;
using DefaultNamespace.Data;
using Objects.Items;
using UnityEngine;

namespace Interfaces
{
	public interface IPlayerItem
	{
		string NameField { get; }
		string DescriptionField { get; }
		Sprite IconField { get; }
		int LevelField { get; }
		ICollection<StatsDisplayData> GetStatsData();
		public bool IsUnlocked(SaveFile saveFile);
	}
}