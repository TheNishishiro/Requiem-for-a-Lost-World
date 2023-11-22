using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Objects.Characters;
using Objects.Stage;
using UnityEngine;

public class BlackFlameComponent : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private BoxCollider _boxCollider;
    private float _totalLifeTime;
    
    void Update()
    {
        _lifeTime -= Time.deltaTime;
        _totalLifeTime += Time.deltaTime;
        if (_totalLifeTime >= 0.1f)
        {
            _boxCollider.enabled = false;
        }
        
        if (_lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        var damage = GameData.GetPlayerCharacterRank() >= CharacterRank.E5 ? 150 : 50; 
        other.GetComponent<Damageable>().TakeDamage(damage * (1+GameData.GetPlayerCharacterData().Stats.DamagePercentageIncrease));
    }
}
