using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Objects.Drops;
using Objects.Stage;
using UnityEngine;

public class GemPickupObject : PickupObject
{
    [SerializeField] private int gemAmount;
		
    public override void OnPickUp(Player player)
    {
        player.AddGems((int)(gemAmount * GameData.GetCurrentDifficulty().PickUpValueModifier));
    }

    public override void SetAmount(int amount)
    {
        if (amount != 0)
            gemAmount = amount;
    }
}
