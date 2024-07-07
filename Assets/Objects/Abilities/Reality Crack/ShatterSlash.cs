using DefaultNamespace;
using UnityEngine;

namespace Objects.Abilities.Reality_Crack
{
    public class ShatterSlash : MonoBehaviour
    {
        [SerializeField] private RealityShatterProjectile logicNode;
        
        private void OnParticleCollision(GameObject other)
        {
            if (!logicNode.gameObject.activeSelf) return;
            if (!other.CompareTag("Enemy")) return;
            var damageable = other.GetComponent<Damageable>();
            logicNode.OnEnemyHit(damageable);
        }
    }
}