using System;
using Events.Scripts;
using Objects.Characters;
using Objects.Enemies;
using Objects.Players.Scripts;
using Objects.Stage;
using UI.Labels.InGame;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
	public class SpecialBarManager : MonoBehaviour
	{
		[SerializeField] private SpecialBar specialBar;
		[SerializeField] private PlayerStatsComponent playerStatsComponent;
		public static SpecialBarManager instance;

		private void Start()
		{
			if (instance == null)
			{
				instance = this;
			}
            
			ResetBar();
			SetMaxValue();
		}
		
		public void SetMaxValue()
		{
			specialBar.SetMax(PlayerStatsScaler.GetScaler().GetSpecialMaxValue());
		}

		public void ResetBar()
		{
			PlayerStatsScaler.GetScaler().ResetSpecialBar();
		}
		
		public bool IsFull()
		{
			return specialBar.IsFull();
		}
		
		public float GetPercentageFulfillment()
		{
			return specialBar.GetPercentage();
		}

		public void Increment()
		{
			PlayerStatsScaler.GetScaler().IncrementSpecial();
			UpdateValue();
		}

		public void Increment(float amount)
		{
			PlayerStatsScaler.GetScaler().IncrementSpecial(amount);
			UpdateValue();
		}

		public void UpdateValue()
		{
			SetMaxValue();
			specialBar.SetValue(PlayerStatsScaler.GetScaler().GetSpecialValue());
		}

		private void Update()
		{
			UpdateValue();
			if (IsFull())
				SpecialBarFilledEvent.Invoke();
		}
	}
}