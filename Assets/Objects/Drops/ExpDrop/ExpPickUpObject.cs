﻿using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

namespace Objects.Drops.ExpDrop
{
	public class ExpPickUpObject : MonoBehaviour, IPickUpObject
	{
		[SerializeField] public int expAmount;
		[SerializeField] private SpriteRenderer gemImage;
		[SerializeField] private SpriteRenderer gemMinimapIcon;
		
		public void OnPickUp(Player player)
		{
			player.AddExperience(expAmount);
		}

		public void SetAmount(int amount)
		{
			if (amount != 0)
			{
				expAmount = amount;
				AdjustColor();
			}
		}

		public void AddExp(int exp)
		{
			expAmount += exp;
			AdjustColor();
		}

		private void AdjustColor()
		{
			var color = GetColorBasedOnExpHeld();
			gemImage.color = color;
			gemMinimapIcon.color = color;
		}

		private Color GetColorBasedOnExpHeld()
		{
			return Color.Lerp(Color.white, Color.red, expAmount / 5000f);
			return expAmount switch
			{
				< 200 => Color.white,
				< 500 => Color.green,
				< 1000 => Color.cyan,
				< 2000 => Color.blue,
				< 5000 => Color.yellow,
				< 10000 => Color.red,
				_ => Color.magenta
			};
		}
	}
}