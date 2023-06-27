using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

namespace Objects.Drops.ExpDrop
{
	public class ExpPickUpObject : MonoBehaviour, IPickUpObject
	{
		[SerializeField] public int expAmount;
		[SerializeField] private SpriteRenderer gemImage;
		
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
			if (expAmount < 200)
				gemImage.color = Color.white;
			else if (expAmount < 500)
				gemImage.color = Color.green;
			else if (expAmount < 1000)
				gemImage.color = Color.blue;
			else if (expAmount < 2000)
				gemImage.color = Color.yellow;
			else if (expAmount < 5000)
				gemImage.color = Color.red;
			else
				gemImage.color = Color.magenta;
		}
	}
}