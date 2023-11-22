using System;
using System.Collections;
using System.Collections.Generic;
using Data.Elements;
using DefaultNamespace;
using Objects.Characters;
using Objects.Stage;
using UnityEngine;

public class SummerSkill : CharacterSkillBase
{
    private float _damage;
    private ElementalWeapon _elementalWeapon;
    private Vector3 _direction;
    
    private void Start()
    {
        _damage = 30 * (1 + GameData.GetPlayerCharacterData().Stats.DamagePercentageIncrease);
        _elementalWeapon = new ElementalWeapon(Element.None);
    }

    public void SetDirection(Vector3 dir, float rotationDegree)
    {
        //_direction = dir;
        transform.rotation = Quaternion.LookRotation(dir.normalized) * Quaternion.Euler(0,rotationDegree,0);
    }
    
    private void Update()
    {
        TickLifeTime();
        transform.position += transform.forward * (5 * Time.deltaTime);
        transform.Rotate(0, 0, Time.deltaTime * 150);
    }

    private void OnCollisionStay(Collision other)
    {
        if (!other.collider.CompareTag("Enemy")) return;
            
        other.collider.GetComponent<Damageable>().TakeDamage(_damage, _elementalWeapon);
    }
}
