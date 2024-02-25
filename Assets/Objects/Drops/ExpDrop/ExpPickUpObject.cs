using Events.Scripts;
using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

namespace Objects.Drops.ExpDrop
{
	public class ExpPickUpObject : PickupObject
	{
		[SerializeField] public float expAmount;
		[SerializeField] private SpriteRenderer gemImage;
		[SerializeField] private SpriteRenderer gemMinimapIcon;
		
		public override void OnPickUp(Player player)
		{
			AudioManager.instance.PlayExperiencePickup();
			player.AddExperience(expAmount);
			ExpPickedUpEvent.Invoke(expAmount);
		}

		public override void SetAmount(int amount)
		{
			if (amount != 0)
			{
				expAmount = amount;
				AdjustColor();
			}
		}

		public void AddExp(float exp)
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
		}
	}
}