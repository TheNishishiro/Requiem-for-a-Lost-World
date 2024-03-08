
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
            targetPosition.OnValueChanged += OnValueChanged;
            base.OnNetworkSpawn();
        }

        public override void OnNetworkDespawn()
        {
            targetPosition.OnValueChanged -= OnValueChanged;
            base.OnNetworkDespawn();
        }

        private void OnValueChanged(Vector3 previousvalue, Vector3 newvalue)
        {
            if (newvalue != Vector3.zero)
                lineRenderer.positionCount = 2;
            else
                lineRenderer.positionCount = 0;
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