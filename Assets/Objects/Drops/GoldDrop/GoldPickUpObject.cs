using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class GoldPickUpObject : MonoBehaviour, IPickUpObject
{
    [SerializeField] private int goldAmount;
		
    public void OnPickUp(Player player)
    {
        player.AddGold(goldAmount);
    }

    public void SetAmount(int amount)
    {
        if (amount != 0)
            goldAmount = amount;
    }
}
