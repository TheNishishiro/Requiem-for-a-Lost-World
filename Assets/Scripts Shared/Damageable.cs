using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Elements;
using Events.Scripts;
using Interfaces;
using Managers;
using Objects.Characters;
using Objects.Stage;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
	public class Damageable : MonoBehaviour, IDamageable
	{
		[SerializeField] public float Health;
		[SerializeField] private GameResultData gameResultData;
		[SerializeField] public GameObject targetPoint;
		[SerializeField] private AudioClip takeDamageSound;
		public Dictionary<GameObject, float> sourceDamageCooldown = new ();
		private Dictionary<int, Coroutine> _activeDots = new();
		public float vulnerabilityTimer;
		public float vulnerabilityPercentage;
		private List<ElementStats> resistances = new ();
		private List<Element> inflictedElements = new ();
		private Transform _transformCache;
		private static bool _hitSoundPlayedThisFrame;

		private void Awake()
		{
			_transformCache = transform;
		}

		public void Clear()
		{
			sourceDamageCooldown.Clear();
			vulnerabilityTimer = 0;
			vulnerabilityPercentage = 0;
			resistances.Clear();
			inflictedElements.Clear();
			StopAllCoroutines();
			_activeDots.Clear();
		}

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
			return resistances.FirstOrDefault(x => x.element == element)?.damageReduction ?? 0;
		}
		
		public void TakeDamage(float damage, WeaponBase weaponBase = null)
		{
			var calculatedDamage = damage;
			if (weaponBase != null)
			{
				calculatedDamage *= 1 - GetResistance(weaponBase.element);
				OnElementInflict(weaponBase.element, damage);
			}

			calculatedDamage = vulnerabilityTimer > 0 ? calculatedDamage * (1 + vulnerabilityPercentage) : calculatedDamage;
			if (calculatedDamage < 0)
				calculatedDamage = 0;

			var position = _transformCache.position;
			if (!_hitSoundPlayedThisFrame)
			{
				AudioSource.PlayClipAtPoint(takeDamageSound, position, 0.5f);
				_hitSoundPlayedThisFrame = true;
			}

			gameResultData.AddDamage(calculatedDamage, weaponBase);
			MessageManager.instance.PostMessage(calculatedDamage.ToString("0"), position, _transformCache.localRotation, ElementService.ElementToColor(weaponBase?.element));
			Health -= calculatedDamage;
			if (IsDestroyed())
				weaponBase?.OnEnemyKilled();
		}

		private void LateUpdate()
		{
			_hitSoundPlayedThisFrame = false;
		}

		public void ReduceElementalDefence(Element element, float amount)
		{
			var elementStat = resistances.FirstOrDefault(x => x.element == element);
			if (elementStat == null)
			{
				elementStat = new ElementStats()
				{
					element = element,
				};
				resistances.Add(elementStat);
			}

			elementStat.damageReduction -= amount;
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
			if (_activeDots.ContainsKey(weaponBase.GetInstanceID())) return;
			_activeDots.Add(weaponBase.GetInstanceID(), StartCoroutine(DamageOverTime(damage, damageFrequency, damageDuration, weaponBase)));
		}

		private IEnumerator DamageOverTime(float damage, float damageFrequency, float damageDuration, WeaponBase weaponBase)
		{
			var timer = damageDuration;
			while (timer > 0)
			{
				timer -= damageFrequency;
				TakeDamage(damage, weaponBase);

				if (GameData.IsCharacterWithRank(CharactersEnum.Natalie_BoW, CharacterRank.E5) && Random.value < 0.5f)
				{
					var values = Enum.GetValues(typeof(Element));
					var randomElement = (Element)values.GetValue(Random.Range(0, values.Length));
					TakeDamage(damage, new ElementalWeapon(randomElement));
					ReduceElementalDefence(randomElement, 0.25f);
				}
				
				yield return new WaitForSeconds(damageFrequency);
			}
			
			DamageOverTimeExpiredHandler.Invoke(this, damage);
			_activeDots.Remove(weaponBase.GetInstanceID());
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
		
		private void OnElementInflict(Element element, float damage)
		{
			if (element == Element.None || element == Element.Physical || inflictedElements.Contains(element))
				return;
			
			inflictedElements.Add(element);

			var reaction = ElementalReactor.GetReaction(inflictedElements);
			inflictedElements.Clear();
			if (reaction == ElementalReaction.None)
				return;
			
			if (reaction == ElementalReaction.Melt)
			{
				SetVulnerable(2, 0.5f);
			}
			
			if (reaction == ElementalReaction.Explosion)
			{
				TakeDamage(damage * 0.35f);
			}
			
			if (reaction == ElementalReaction.Swirl)
			{
				if (resistances.FirstOrDefault(x => x.element == element)?.damageReduction != null)
					resistances.First(x => x.element == element).damageReduction -= 0.1f;
			}
			
			if (reaction == ElementalReaction.Collapse)
			{
				foreach (var resistance in resistances)
				{
					resistance.damageReduction *= 0.1f;
				}
			}

			if (reaction == ElementalReaction.Erode)
			{
				SetVulnerable(1, 0.1f);
				TakeDamage(Health * 0.05f);
			}
		}
	}
}