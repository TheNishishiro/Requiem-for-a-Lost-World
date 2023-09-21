using System;
using System.Collections;
using System.Collections.Generic;
using Data.Elements;
using DefaultNamespace;
using Objects.Characters;
using Objects.Enemies;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;

public class NatalieSkill : CharacterSkillBase
{
	private ElementalWeapon _elementalWeapon;
	private PlayerStatsComponent _playerStatsComponent;
	
	private void Start()
	{
		_elementalWeapon = new ElementalWeapon(Element.Wind);
		_playerStatsComponent = FindObjectOfType<PlayerStatsComponent>();
		StartCoroutine(Expand());
	}

	public void OnCollisionEnter(Collision other)
	{
		if (!other.collider.CompareTag("Enemy"))
			return;
				
		var damageComponent = other.collider.GetComponent<Damageable>();
		damageComponent.ReduceElementalDefence(Element.Wind, 0.5f);
		damageComponent.ApplyDamageOverTime(_playerStatsComponent.GetDamageOverTime(), 1f, 5f, _elementalWeapon);
	}
	
	private IEnumerator Expand()
	{
		var scale = transform.localScale;
		var targetScale = scale * 5;
		var time = 0f;
		while (time < 1f)
		{
			time += Time.deltaTime;
			transform.localScale = Vector3.Lerp(scale, targetScale, time);
			yield return new WaitForSeconds(0.01f);
		}

		OnDestroy();
		yield return null;
	}
}
