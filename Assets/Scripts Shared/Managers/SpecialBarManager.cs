using System;
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
		[SerializeField] private UnityEvent onSpecialBarFull;

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

		private void Update()
		{
			if (IsFull())
				onSpecialBarFull?.Invoke();
		}
		
		public void OnEnemyKilled(Enemy enemy)
		{
			if (GameData.GetPlayerCharacterId() != CharactersEnum.Nishi)
				return;
			
			specialBar.Increment(GameData.GetPlayerCharacterData().SpecialIncrementValue);
		}
	}
}