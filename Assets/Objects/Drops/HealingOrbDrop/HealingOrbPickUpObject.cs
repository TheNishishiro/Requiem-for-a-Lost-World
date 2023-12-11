using Unity.VisualScripting;
using UnityEngine;

namespace Objects.Drops.HealingOrbDrop
{
    public class HealingOrbPickUpObject : PickupObject
    {
        [SerializeField] private float healAmount;
        
        public override void OnPickUp(Player player)
        {
            player.healthComponent.Damage(-healAmount);
        }

        public override void SetAmount(int amount)
        {
            healAmount = amount;
        }
    }
}