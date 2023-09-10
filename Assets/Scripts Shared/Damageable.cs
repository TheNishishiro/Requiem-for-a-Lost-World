using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Elements;
using Interfaces;
using Managers;
using Objects.Stage;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

namespace DefaultNamespace
{
	public class Damageable : MonoBehaviour, IDamageable
	{
		[HideInInspector] public float Health;
		[SerializeField] private GameResultData gameResultData;
		public Dictionary<GameObject, float> sourceDamageCooldown = new ();
		public float vulnerabilityTimer;
		public float vulnerabilityPercentage;
		private List<ElementStats> resistances = new ();

		private void Update()
		{
			if (vulnerabilityTimer > 0)
				vulnerabilityTimer -= Time.deltaTime;
			
			if (Time.frameCount % 60 != 0) return;
			
			for (var i = 0; i < sourceDamageCooldown.Count; i++)
			{
				var go = sourceDamageCooldown.ElementAt(i).Key;

				if (go != null && go.activeSelf && !go.IsDestroyed()) continue;

				sourceDamageCooldown.Remove(go);
				i--;
			}
		}

		public void SetHealth(float health)
		{
			Health = health;
		}

		public void SetResistances(List<ElementStats> statsElementStats)
		{
			resistances = statsElementStats;
		}

		private float GetResistance(Element element)
		{
			return resistances?.FirstOrDefault(x => x.element == element)?.damageReduction ?? 0;
		}
		
		public void TakeDamage(float damage, WeaponBase weaponBase = null)
		{
			var calculatedDamage = damage;
			if (weaponBase != null)
			{
				calculatedDamage *= 1 - GetResistance(weaponBase.element);
			}

			calculatedDamage = vulnerabilityTimer > 0 ? calculatedDamage * (1 + vulnerabilityPercentage) : calculatedDamage;
			if (calculatedDamage < 0)
				calculatedDamage = 0;
			
			gameResultData.AddDamage(calculatedDamage, weaponBase);
			MessageManager.instance.PostMessage(calculatedDamage.ToString("0"), transform.position, transform.localRotation);
			Health -= calculatedDamage;
		}
		
		public void TakeDamageWithCooldown(float damage, GameObject damageSource, float damageCooldown, WeaponBase weaponBase)
		{
			if (!sourceDamageCooldown.ContainsKey(damageSource))
			{
				sourceDamageCooldown.Add(damageSource, damageCooldown);
				TakeDamage(damage, weaponBase);
				return;
			}
			
			sourceDamageCooldown[damageSource] -= Time.deltaTime;

			if (sourceDamageCooldown[damageSource] > 0) 
				return;
			
			TakeDamage(damage, weaponBase);
			sourceDamageCooldown[damageSource] = damageCooldown;
		}

		public void ApplyDamageOverTime(float damage, float damageFrequency, float damageDuration, WeaponBase weaponBase)
		{
			StartCoroutine(DamageOverTime(damage, damageFrequency, damageDuration, weaponBase));
		}

		private IEnumerator DamageOverTime(float damage, float damageFrequency, float damageDuration, WeaponBase weaponBase)
		{
			var timer = damageDuration;
			while (timer > 0)
			{
				timer -= Time.deltaTime;
				TakeDamage(damage, weaponBase);
				yield return new WaitForSeconds(damageFrequency);
			}
		}

		public bool IsDestroyed()
		{
			return Health <= 0;
		}
		
		public void SetVulnerable(float time, float percentage)
		{
			vulnerabilityTimer = time;
			vulnerabilityPercentage = percentage;
		}
	}
}