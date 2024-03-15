using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Elements;
using DefaultNamespace.Data.Weapons;
using Events.Scripts;
using Interfaces;
using Managers;
using NaughtyAttributes;
using Objects.Characters;
using Objects.Stage;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using Weapons;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
	public class Damageable : NetworkBehaviour, IDamageable
	{
		[SerializeField] public float Health;
		[SerializeField] private GameResultData gameResultData;
		[SerializeField] public GameObject targetPoint;
		[SerializeField] private AudioClip takeDamageSound;
		[SerializeField] private bool canBeAfflictedWithElements;
		[ShowIf("canBeAfflictedWithElements")]
		[SerializeField] private ChaseComponent chaseComponent;
		[ShowIf("canBeAfflictedWithElements")]
		[SerializeField] private VisualEffect fireParticles;
		[ShowIf("canBeAfflictedWithElements")]
		[SerializeField] private VisualEffect lightningParticles;
		[ShowIf("canBeAfflictedWithElements")]
		[SerializeField] private VisualEffect iceParticles;
		[ShowIf("canBeAfflictedWithElements")]
		[SerializeField] private VisualEffect lightParticles;
		[ShowIf("canBeAfflictedWithElements")]
		[SerializeField] private VisualEffect cosmicParticles;
		[ShowIf("canBeAfflictedWithElements")]
		[SerializeField] private VisualEffect earthParticles;
		[ShowIf("canBeAfflictedWithElements")]
		[SerializeField] private VisualEffect meltParticles;
		[ShowIf("canBeAfflictedWithElements")]
		[SerializeField] private VisualEffect explosionParticles;
		[ShowIf("canBeAfflictedWithElements")]
		[SerializeField] private VisualEffect swirlParticles;
		[ShowIf("canBeAfflictedWithElements")]
		[SerializeField] private VisualEffect shockParticles;
		[ShowIf("canBeAfflictedWithElements")]
		[SerializeField] private VisualEffect erodeParticles;
		public Dictionary<GameObject, float> sourceDamageCooldown = new ();
		private Dictionary<int, Coroutine> _activeDots = new();
		public float vulnerabilityTimer;
		public float vulnerabilityPercentage;
		public float additionalDamageTimer;
		public float additionalDamageModifier;
		public ElementalWeapon additionalDamageType;
		private List<ElementStats> resistances = new ();
		private List<Element> inflictedElements = new ();
		private Transform _transformCache;
		private Transform _targetTransformCache;
		private static bool _hitSoundPlayedThisFrame;

		private Dictionary<Element, VisualEffect> _elementVfxMap;
		
		private void Awake()
		{
			_transformCache = transform;
			_targetTransformCache = targetPoint == null ? _transformCache : targetPoint.transform;
			_elementVfxMap = new Dictionary<Element, VisualEffect>()
			{
				{ Element.Fire, fireParticles},
				{ Element.Lightning, lightningParticles},
				{ Element.Ice, iceParticles},
				{ Element.Light, lightParticles},
				{ Element.Cosmic, cosmicParticles},
			};
		}

		private void OnDisable()
		{
			Clear();
		}

		public void Clear()
		{
			sourceDamageCooldown.Clear();
			vulnerabilityTimer = 0;
			vulnerabilityPercentage = 0;
			additionalDamageTimer = 0;
			additionalDamageModifier = 0;
			resistances.Clear();
			inflictedElements.Clear();
			foreach (var activeDot in _activeDots.Values)
			{
				StopCoroutine(activeDot);
			}

			if (canBeAfflictedWithElements)
			{
				foreach (var elementVfx in _elementVfxMap)
				{
					elementVfx.Value.gameObject.SetActive(false);
				}
			}

			_activeDots.Clear();
		}

		private void Update()
		{
			if (vulnerabilityTimer > 0)
				vulnerabilityTimer -= Time.deltaTime;
			
			if (additionalDamageTimer > 0)
				additionalDamageTimer -= Time.deltaTime;
			
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

		public void TakeDamage(float damage, WeaponBase weaponBase = null, bool isRecursion = false)
		{
			TakeDamage(new DamageResult() { Damage = damage }, weaponBase, isRecursion);
		}
        
		public void TakeDamage(DamageResult damageResult, WeaponBase weaponBase = null, bool isRecursion = false)
		{
			var calculatedDamage = damageResult.Damage;
			var isWeaponSpecified = weaponBase != null;
			if (isWeaponSpecified)
			{
				calculatedDamage *= 1 - GetResistance(weaponBase.element);
				OnElementInflict(weaponBase.element, damageResult.Damage);
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

			if (!isRecursion && additionalDamageTimer > 0)
			{
				TakeDamage(new DamageResult{Damage = damageResult.Damage * additionalDamageModifier, IsCriticalHit = damageResult.IsCriticalHit}, additionalDamageType, true);
			}

			if (isWeaponSpecified && weaponBase.WeaponStatsStrategy != null)
			{
				var lifeSteal = weaponBase.WeaponStatsStrategy.GetLifeSteal();
				if (lifeSteal != 0)
					GameManager.instance.playerComponent.TakeDamage(-calculatedDamage * lifeSteal, true, true);

				var healPerHit = weaponBase.WeaponStatsStrategy.GetHealPerHit(false);
				if (healPerHit != 0)
					GameManager.instance.playerComponent.TakeDamage(-healPerHit, true, true);
			}

			gameResultData.AddDamage(calculatedDamage, weaponBase);
			var damageMessage = calculatedDamage.ToString("0");
			if (damageResult.IsCriticalHit)
				damageMessage += "!";
			if (IsHost || !IsSpawned)
				Health -= calculatedDamage;
			else
				RpcManager.instance.DealDamageToEnemyRpc(this, calculatedDamage);
			MessageManager.instance.PostMessageRpc(damageMessage, _targetTransformCache.position, _transformCache.localRotation, ElementService.ElementToColor(weaponBase?.element));
			DamageDealtEvent.Invoke(this, calculatedDamage, isRecursion);
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
			TakeDamageWithCooldown(new DamageResult() { Damage = damage }, damageSource, damageCooldown, weaponBase);
		}

		public void TakeDamageWithCooldown(DamageResult damageResult, GameObject damageSource, float damageCooldown, WeaponBase weaponBase)
		{
			if (!sourceDamageCooldown.ContainsKey(damageSource))
			{
				sourceDamageCooldown.Add(damageSource, damageCooldown);
				TakeDamage(damageResult, weaponBase);
				return;
			}
			
			sourceDamageCooldown[damageSource] -= Time.deltaTime;

			if (sourceDamageCooldown[damageSource] > 0) 
				return;
			
			TakeDamage(damageResult, weaponBase);
			sourceDamageCooldown[damageSource] = damageCooldown;
		}

		public void ApplyDamageOverTime(float damage, float damageFrequency, float damageDuration, WeaponBase weaponBase)
		{
			if (_activeDots.ContainsKey(weaponBase.GetInstanceID())) return;
			
			_activeDots.Add(weaponBase.GetInstanceID(), null);
			_activeDots[weaponBase.GetInstanceID()] = StartCoroutine(DamageOverTime(damage, damageFrequency, damageDuration, weaponBase));
		}

		private IEnumerator DamageOverTime(float damage, float damageFrequency, float damageDuration, WeaponBase weaponBase)
		{
			var timer = damageDuration;
			var waitTimer = new WaitForSeconds(damageFrequency);
			var tempWeapon = new ElementalWeapon(Element.None);
			while (timer > 0)
			{
				if (!_activeDots.ContainsKey(weaponBase.GetInstanceID())) yield break;
				
				timer -= damageFrequency;
				TakeDamage(new DamageResult{Damage = damage}, weaponBase);

				if (GameData.IsCharacterWithRank(CharactersEnum.Natalie_BoW, CharacterRank.E5) && Random.value < 0.5f)
				{
					var values = Enum.GetValues(typeof(Element));
					var randomElement = (Element)values.GetValue(Random.Range(0, values.Length));
					tempWeapon.SetElement(randomElement);
					TakeDamage(new DamageResult{Damage = damage}, tempWeapon);
					ReduceElementalDefence(randomElement, 0.25f);
				}
				
				yield return waitTimer;
			}
			
			DamageOverTimeExpiredHandler.Invoke(this, damage);
			_activeDots.Remove(weaponBase.GetInstanceID());
		}

		public bool IsDestroyed()
		{
			return (IsHost || !IsSpawned) && Health <= 0;
		}
		
		public void SetVulnerable(float time, float percentage)
		{
			vulnerabilityTimer = time;
			vulnerabilityPercentage = percentage;
		}

		public void SetTakeAdditionalDamageFromAllSources(ElementalWeapon weapon, float duration, float modifier)
		{
			additionalDamageType = weapon;
			additionalDamageTimer = duration;
			additionalDamageModifier = modifier;
		}

		private IEnumerator PlayVfx(VisualEffect vfx, float time)
		{
			vfx.gameObject.SetActive(true);
			yield return new WaitForSeconds(time);
			vfx.gameObject.SetActive(false);
		}
		
		private void OnElementInflict(Element element, float damage)
		{
			if (!canBeAfflictedWithElements || element == Element.None || element == Element.Physical || inflictedElements.Contains(element) || (element == Element.Wind && inflictedElements.Count == 0))
				return;
			
			inflictedElements.Add(element);

			var reactionResult = ElementalReactor.GetReaction(inflictedElements);
			if (_elementVfxMap.ContainsKey(element))
				_elementVfxMap[element].gameObject.SetActive(true);
			if (_elementVfxMap.ContainsKey(reactionResult.removedA))
				_elementVfxMap[reactionResult.removedA].gameObject.SetActive(false);
			if (_elementVfxMap.ContainsKey(reactionResult.removedB))
				_elementVfxMap[reactionResult.removedB].gameObject.SetActive(false);
			
			switch (reactionResult.reaction)
			{
				case ElementalReaction.None:
					return;
				case ElementalReaction.Melt:
					SetVulnerable(2, 0.5f);
					break;
				case ElementalReaction.Explosion:
					TakeDamage(new DamageResult{Damage = damage * 0.35f});
					break;
				case ElementalReaction.Annihilation :
					if (Random.value >= 0.5f)
						TakeDamage(new DamageResult{Damage = damage});
					break;
				case ElementalReaction.Swirl:
				{
					ReduceElementalDefence(reactionResult.removedA == Element.Wind ? reactionResult.removedB : reactionResult.removedA, 0.1f);
					break;
				}
				case ElementalReaction.Collapse:
				{
					foreach (var resistance in resistances)
					{
						resistance.damageReduction *= 0.1f;
					}

					break;
				}
				case ElementalReaction.Erode:
					SetVulnerable(1, 0.1f);
					TakeDamage(new DamageResult{Damage = Health * 0.05f});
					break;
				case ElementalReaction.Shock:
					chaseComponent.SetImmobile(0.5f);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			MessageManager.instance.PostMessage(reactionResult.reaction.ToString(), _targetTransformCache.position, _transformCache.localRotation, ElementService.ElementToColor(element));
		}
	}
}