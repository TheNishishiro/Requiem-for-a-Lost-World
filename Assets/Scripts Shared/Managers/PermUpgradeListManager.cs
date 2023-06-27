using System.Collections.Generic;
using Objects.Players.PermUpgrades;
using UnityEngine;

namespace Managers
{
	public class PermUpgradeListManager : MonoBehaviour
	{
		public static PermUpgradeListManager instance;
		
		private void Awake()
		{
			if (instance == null)
				instance = this;
		}
		
		[SerializeField] private List<PermUpgrade> upgrades;

		public List<PermUpgrade> GetUpgrades()
		{
			return upgrades;
		}
	}
}