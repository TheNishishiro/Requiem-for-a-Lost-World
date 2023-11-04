using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Objects.Drops;
using UnityEngine;

public class GoldPickUpObject : PickupObject
{
    [SerializeField] private int goldAmount;
		
    public override void OnPickUp(Player player)
    {
        player.AddGold(goldAmount);
    }

    public override void SetAmount(int amount)
    {
        if (amount != 0)
            goldAmount = amount;
    }
}
