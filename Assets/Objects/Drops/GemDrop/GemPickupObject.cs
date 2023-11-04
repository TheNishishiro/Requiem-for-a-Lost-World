using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Objects.Drops;
using UnityEngine;

public class GemPickupObject : PickupObject
{
    [SerializeField] private int gemAmount;
		
    public override void OnPickUp(Player player)
    {
        player.AddGems(gemAmount);
    }

    public override void SetAmount(int amount)
    {
        if (amount != 0)
            gemAmount = amount;
    }
}
