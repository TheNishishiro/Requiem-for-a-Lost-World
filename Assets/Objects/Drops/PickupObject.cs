using Unity.Netcode;
using UnityEngine;

namespace Objects.Drops
{
    public abstract class PickupObject : NetworkBehaviour
    {
        public abstract void OnPickUp(Player player);
        public abstract void SetAmount(int amount);
    }
}