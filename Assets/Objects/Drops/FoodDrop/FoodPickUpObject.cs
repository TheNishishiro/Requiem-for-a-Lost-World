using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class FoodPickUpObject : MonoBehaviour, IPickUpObject
{
    [SerializeField] private int healAmount;
		
    public void OnPickUp(Player player)
    {
        player.TakeDamage(-healAmount);
    }

    public void SetAmount(int amount)
    {
        if (amount != 0)
            healAmount = amount;
    }
}
