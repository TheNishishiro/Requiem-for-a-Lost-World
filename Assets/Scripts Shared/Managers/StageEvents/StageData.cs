using System.Collections.Generic;
using UnityEngine;

namespace Managers.StageEvents
{
	[CreateAssetMenu]
	public class StageData: ScriptableObject
	{ 
		public List<StageEvent> stageEvents;
	}
}