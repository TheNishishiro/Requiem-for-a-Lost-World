using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Objects.Drops;
using UnityEngine;

public class FoodPickUpObject : PickupObject
{
    [SerializeField] private int healAmount;
		
    public override void OnPickUp(Player player)
    {
        player.TakeDamage(-healAmount);
    }

    public override void SetAmount(int amount)
    {
        if (amount != 0)
            healAmount = amount;
    }
}
