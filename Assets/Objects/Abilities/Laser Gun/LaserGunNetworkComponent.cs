
using System;
using Unity.Netcode;
using UnityEngine;

namespace Objects.Abilities.Laser_Gun
{
    public class LaserGunNetworkComponent : NetworkBehaviour
    {
        [SerializeField] public LineRenderer lineRenderer;
        [SerializeField] public Transform laserFirePoint;
        public NetworkVariable<Vector3> targetPosition = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public override void OnNetworkSpawn()
        {
            lineRenderer.positionCount = 0;
            base.OnNetworkSpawn();
        }

        public void SetTarget(Vector3 target)
        {
            if (IsOwner)
            {
                targetPosition.Value = target;
            }
        }

        private void Update()
        {
            if (lineRenderer.positionCount < 2)
                return;
            
            lineRenderer.SetPosition(0, laserFirePoint.position);
            lineRenderer.SetPosition(1, targetPosition.Value);
        }
    }
}