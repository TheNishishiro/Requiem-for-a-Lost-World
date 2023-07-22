using System;
using Events.Scripts;
using Objects.Characters;
using Objects.Enemies;
using Objects.Stage;
using UI.Labels.InGame;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
	public class SpecialBarManager : MonoBehaviour
	{
		[SerializeField] private SpecialBar specialBar;

		private void Awake()
		{
			ResetBar();
			SetMaxValue(GameData.GetPlayerCharacterData().SpecialMaxValue);
		}
		public void SetMaxValue(float maxValue)
		{
			specialBar.SetMax(maxValue);
		}

		public void ResetBar()
		{
			specialBar.SetValue(0);
		}
		
		public bool IsFull()
		{
			return specialBar.IsFull();
		}

		public void Increment()
		{
			specialBar.Increment(GameData.GetPlayerCharacterData().SpecialIncrementValue);
		}

		public void Increment(float amount)
		{
			specialBar.Increment(amount);
		}

		private void Update()
		{
			if (IsFull())
				SpecialBarFilledEvent.Invoke();
		}
	}
}