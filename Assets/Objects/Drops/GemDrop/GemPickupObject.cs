using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class GemPickupObject : MonoBehaviour, IPickUpObject
{
    [SerializeField] private int gemAmount;
		
    public void OnPickUp(Player player)
    {
        player.AddGems(gemAmount);
    }

    public void SetAmount(int amount)
    {
        if (amount != 0)
            gemAmount = amount;
    }
}
