using System;
using Objects.Items;
using UnityEngine.Serialization;

namespace Data.ToggleableEntries
{
	[Serializable]
	public class ItemToggleableEntry : ToggleableEntry
	{
		public ItemBase itemBase;
	}
}