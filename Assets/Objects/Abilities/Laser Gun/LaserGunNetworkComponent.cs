using System.Numerics;
using Unity.Netcode;

namespace Objects.Abilities.Laser_Gun
{
    public class LaserGunNetworkComponent : NetworkBehaviour
    {
        public NetworkVariable<Vector3> targetPosition = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public void SetTarget(Vector3 target)
        {
            if (IsOwner)
            {
                targetPosition.Value = target;
            }
        }
    }
}