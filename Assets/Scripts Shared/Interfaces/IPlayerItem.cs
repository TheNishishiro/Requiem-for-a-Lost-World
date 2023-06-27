using UnityEngine;

namespace Interfaces
{
	public interface IPlayerItem
	{
		string NameField { get; }
		string DescriptionField { get; }
		Sprite IconField { get; }
		int LevelField { get; }
	}
}