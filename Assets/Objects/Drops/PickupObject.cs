using UnityEngine;

namespace Objects.Drops
{
    public abstract class PickupObject : MonoBehaviour
    {
        public abstract void OnPickUp(Player player);
        public abstract void SetAmount(int amount);
    }
}