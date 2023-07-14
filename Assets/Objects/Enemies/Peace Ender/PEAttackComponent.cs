using System;
using UnityEngine;

namespace Objects.Enemies.Peace_Ender
{
	public class PEAttackComponent : MonoBehaviour
	{
		private ChaseComponent _chaseComponent;

		private void Awake()
		{
			_chaseComponent = GetComponentInParent<ChaseComponent>();
		}
	}
}