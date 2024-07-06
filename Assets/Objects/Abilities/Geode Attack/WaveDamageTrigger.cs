using DefaultNamespace;
using Objects.Characters;
using Objects.Stage;
using UnityEngine;

namespace Objects.Abilities.Geode_Attack
{
    public class WaveDamageTrigger : MonoBehaviour
    {
        [SerializeField] private GeodeProjectile logicNode;
        
        private void OnParticleCollision(GameObject other)
        {
            if (!logicNode.gameObject.activeSelf) return;
            if (!other.CompareTag("Enemy")) return;
            var damageable = other.GetComponent<Damageable>();
            logicNode.OnEnemyHit(damageable);
        }
    }
}