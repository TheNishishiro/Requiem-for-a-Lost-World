using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Elements;
using DefaultNamespace.Data.Combat;
using DefaultNamespace.Data.Weapons;
using Events.Scripts;
using Interfaces;
using Managers;
using NaughtyAttributes;
using Objects;
using Objects.Characters;
using Objects.Players.Scripts;
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
		[SerializeField] public bool isNetworkObject;
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
		public Dictionary<WeaponEnum, VulnerabilityData> _vulnerability = new ();
		private Dictionary<int, Coroutine> _activeDots = new();
		private float vulnerabilityTimer;
		public float additionalDamageTimer;
		public float additionalDamageModifier;
		public Element additionalDamageType;
		private Dictionary<Element, float> resistances = new ();
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
			_vulnerability.Clear();
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
			if (this.IsServer() && _vulnerability.Count > 0)
				vulnerabilityTimer += Time.deltaTime;
			
			if (additionalDamageTimer > 0)
				additionalDamageTimer -= Time.deltaTime;
			
			if (Time.frameCount % 60 != 0) return;

			if (this.IsServer())
			{
				var vulnerabilitySources = _vulnerability.Keys.ToList();
				foreach (var vulnerabilitySource in vulnerabilitySources)
				{
					var currentVulnerability = _vulnerability[vulnerabilitySource];
					currentVulnerability.vulnerabilityDuration -= vulnerabilityTimer;
					if (currentVulnerability.vulnerabilityDuration <= 0)
						_vulnerability.Remove(vulnerabilitySource);
				}

				vulnerabilityTimer = 0;
			}
			
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
			resistances = new Dictionary<Element, float>();
			foreach (var statsElement in statsElementStats)
			{
				if (resistances.ContainsKey(statsElement.element))
					resistances[statsElement.element] += statsElement.damageReduction;
				else
					resistances.Add(statsElement.element, statsElement.damageReduction);
			}
		}

		private float GetResistance(Element element, CharactersEnum characterId, CharacterRank characterRank)
		{
			if (!this.IsServer())
				throw new Exception("Resistances can only be obtained by the server");
			
			resistances.TryAdd(element, 0);
			var resistance = resistances[element];
			if (characterId == CharactersEnum.Chornastra_BoR && characterRank == CharacterRank.E4)
				return resistance < 0 ? resistance : 0;

			return resistance;
		}

		public void TakeDamage(float damage, IWeapon weaponBase = null, bool isRecursion = false)
		{
			TakeDamage(new DamageResult() { Damage = damage }, weaponBase, isRecursion);
		}

		public void TakeDamage(DamageResult damageResult, IWeapon weaponBase = null, bool isRecursion = false)
		{
			var elementalReactionEffectIncreasePercentage = PlayerStatsScaler.GetScaler().GetElementalReactionEffectIncreasePercentage();
			if (!_hitSoundPlayedThisFrame)
			{
				AudioSource.PlayClipAtPoint(takeDamageSound, _transformCache.position, 0.5f);
				_hitSoundPlayedThisFrame = true;
			}

			if (isNetworkObject)
				RpcManager.instance.DealDamageToEnemyRpc(this, damageResult.Damage, damageResult.IsCriticalHit,
					weaponBase?.GetElement() ?? Element.None, (WeaponEnum?)weaponBase?.GetId() ?? WeaponEnum.Scythe,
					elementalReactionEffectIncreasePercentage, GameData.GetPlayerCharacterId(), GameData.GetPlayerCharacterRank(), isRecursion, NetworkManager.Singleton.LocalClientId);
			else
				TakeDamageServer(damageResult.Damage, damageResult.IsCriticalHit, weaponBase?.GetElement() ?? Element.None,
					(WeaponEnum?)weaponBase?.GetId() ?? WeaponEnum.Scythe, elementalReactionEffectIncreasePercentage, 
					GameData.GetPlayerCharacterId(), GameData.GetPlayerCharacterRank(),
					isRecursion, NetworkManager.Singleton.LocalClientId);
		}
		
		public void TakeDamageServer(float baseDamage, bool isCriticalHit, Element weaponElement, WeaponEnum weaponId, float elementalReactionEffectIncreasePercentage, CharactersEnum characterId, CharacterRank characterRank, bool isRecursion, ulong clientId)
		{
			if (IsDestroyed())
				return;
			
			var calculatedDamage = baseDamage;
			var isWeaponSpecified = weaponElement != Element.None;
			if (isWeaponSpecified)
			{
				calculatedDamage *= 1 - GetResistance(weaponElement, characterId, characterRank);
				OnElementInflict(weaponElement, baseDamage, elementalReactionEffectIncreasePercentage, clientId);
			}

			calculatedDamage = vulnerabilityTimer > 0 ? calculatedDamage * (1 + _vulnerability.Values.Sum(x => x.vulnerabilityAmount)) : calculatedDamage;
			if (calculatedDamage < 0)
				calculatedDamage = 0;

			if (!isRecursion && additionalDamageTimer > 0)
			{
				TakeDamageServer(baseDamage * additionalDamageModifier, isCriticalHit, additionalDamageType, WeaponEnum.Unset, elementalReactionEffectIncreasePercentage, characterId, characterRank, true, clientId);
			}
			
			if (isWeaponSpecified && weaponId != WeaponEnum.Unset)
				RpcManager.instance.TriggerLifeStealRpc(calculatedDamage, weaponId, RpcTarget.Single(clientId, RpcTargetUse.Temp));
			
			var damageMessage = calculatedDamage.ToString("0");
			if (isCriticalHit)
				damageMessage += "!";
			Health -= calculatedDamage;
			MessageManager.instance.PostMessageRpc(damageMessage, _targetTransformCache.position, _transformCache.localRotation, ElementService.ElementToColor(weaponElement));
			
			if (weaponId != WeaponEnum.Unset && isNetworkObject)
				RpcManager.instance.InvokeDamageDealtEventRpc(this, calculatedDamage, isRecursion, weaponId, RpcTarget.Single(clientId, RpcTargetUse.Temp));

			if (IsDestroyed())
				RpcManager.instance.WeaponKilledEnemyRpc(weaponId, RpcTarget.Single(clientId, RpcTargetUse.Temp));
		}

		private void LateUpdate()
		{
			_hitSoundPlayedThisFrame = false;
		}

		public void TakeDamageWithCooldown(float damage, GameObject damageSource, float damageCooldown, IWeapon weaponBase, bool isRecursion = false)
		{
			TakeDamageWithCooldown(new DamageResult() { Damage = damage }, damageSource, damageCooldown, weaponBase, isRecursion);
		}

		public void TakeDamageWithCooldown(DamageResult damageResult, GameObject damageSource, float damageCooldown, IWeapon weaponBase, bool isRecursion = false)
		{
			if (sourceDamageCooldown.TryAdd(damageSource, damageCooldown))
			{
				TakeDamage(damageResult, weaponBase, isRecursion);
				return;
			}
			
			sourceDamageCooldown[damageSource] -= Time.deltaTime;

			if (sourceDamageCooldown[damageSource] > 0) 
				return;
			
			TakeDamage(damageResult, weaponBase, isRecursion);
			sourceDamageCooldown[damageSource] = damageCooldown;
		}

		public void ApplyDamageOverTime(float damage, float damageFrequency, float damageDuration, IWeapon weaponBase)
		{
			if (_activeDots.ContainsKey(weaponBase.GetInstanceID())) return;
			
			_activeDots.Add(weaponBase.GetInstanceID(), null);
			_activeDots[weaponBase.GetInstanceID()] = StartCoroutine(DamageOverTime(damage, damageFrequency, damageDuration, weaponBase));
		}

		private IEnumerator DamageOverTime(float damage, float damageFrequency, float damageDuration, IWeapon weaponBase)
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
			return (this.IsServer()) && Health <= 0;
		}

		public void ReduceElementalDefence(Element element, float amount)
		{
			RpcManager.instance.ReduceEnemyResistanceRpc(this, element, amount);
		}

		public void ReduceElementalDefenceServer(Element element, float amount)
		{
			if (!this.IsServer())
				throw new Exception("ReduceElementalDefence called outside of a server");
			
			resistances.TryAdd(element, 0);
			resistances[element] -= amount;
		}

		public void SetVulnerable(WeaponEnum weaponId, float time, float percentage)
		{
			RpcManager.instance.SetEnemyVulnerableRpc(this, weaponId, time, percentage);
		}
		
		public void SetVulnerableServer(WeaponEnum weaponId, float time, float percentage)
		{
			if (!this.IsServer())
				throw new Exception("SetVulnerable called outside of a server");
			
			_vulnerability.TryAdd(weaponId, new VulnerabilityData()
			{
				vulnerabilityAmount = percentage,
				vulnerabilityDuration = time
			});
		}

		public void SetTakeAdditionalDamageFromAllSources(ElementalWeapon weapon, float duration, float modifier)
		{
			RpcManager.instance.EnemyTakeAdditionalDamageAsFollowUpRpc(this, weapon.element, duration, modifier);
		}

		public void SetTakeAdditionalDamageFromAllSourcesServer(Element element, float duration, float modifier)
		{
			if (!this.IsServer())
				throw new Exception("SetTakeAdditionalDamageFromAllSources called outside of a server");
			
			additionalDamageType = element;
			additionalDamageTimer = duration;
			additionalDamageModifier = modifier;
		}

		private IEnumerator PlayVfx(VisualEffect vfx, float time)
		{
			vfx.gameObject.SetActive(true);
			yield return new WaitForSeconds(time);
			vfx.gameObject.SetActive(false);
		}

		public Vector3 GetTargetPosition()
		{
			return _targetTransformCache.position;
		}
		
		private void OnElementInflict(Element element, float damage, float elementalReactionEffectIncrease, ulong clientId)
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
					SetVulnerable(WeaponEnum.Unset, 2, 0.5f * elementalReactionEffectIncrease);
					break;
				case ElementalReaction.Explosion:
					TakeDamage(new DamageResult{Damage = damage * (0.35f * elementalReactionEffectIncrease)});
					break;
				case ElementalReaction.Annihilation :
					if (Random.value >= 0.5f)
						TakeDamage(new DamageResult{Damage = damage * elementalReactionEffectIncrease});
					break;
				case ElementalReaction.Swirl:
				{
					ReduceElementalDefence(reactionResult.removedA == Element.Wind ? reactionResult.removedB : reactionResult.removedA, 0.1f * elementalReactionEffectIncrease);
					break;
				}
				case ElementalReaction.Collapse:
				{
					var keys = resistances.Select(x => x.Key);
					foreach (var resistance in keys)
					{
						resistances[resistance] *= 0.1f * elementalReactionEffectIncrease;
					}

					break;
				}
				case ElementalReaction.Erode:
					SetVulnerable(WeaponEnum.Unset, 1, 0.1f * elementalReactionEffectIncrease);
					TakeDamage(new DamageResult{Damage = Health * (0.05f * elementalReactionEffectIncrease)});
					break;
				case ElementalReaction.Shock:
					chaseComponent.SetImmobile(0.5f * elementalReactionEffectIncrease);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			RpcManager.instance.InvokeReactionTriggeredEventRpc(this, reactionResult.reaction, RpcTarget.Single(clientId, RpcTargetUse.Temp));
			MessageManager.instance.PostMessageRpc(reactionResult.reaction.ToString(), _targetTransformCache.position, _transformCache.localRotation, ElementService.ElementToColor(element));
		}
	}
}