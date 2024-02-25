using System;
using System.Collections;
using Cinemachine;
using Data.Elements;
using DefaultNamespace;
using Objects.Abilities.Laser_Gun;
using Objects.Players.Scripts;
using Objects.Stage;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Objects.Characters.Amelia.Skill
{
	public class AmeliaProjectile : CharacterSkillBase
	{
		[SerializeField] private AmeliaExplosion explosion;
		[SerializeField] private LaserGunProjectile construct;
		private PlayerStatsComponent _playerStatsComponent;
		private bool _isExploded;
		[SerializeField] private float timeUntilExplosion = 0.8f;
		[SerializeField] private float explosionTime = 3f;
		[SerializeField] private Renderer renderer;
		private ElementalWeapon _elementalWeapon;

		private void Start()
		{
			_elementalWeapon = new ElementalWeapon(Element.Light);
			_playerStatsComponent = FindFirstObjectByType<PlayerStatsComponent>();
			StartCoroutine(Glitch());
		}

		public void Update()
		{
			if (!_isExploded)
			{
				timeUntilExplosion -= Time.deltaTime;
				transform.position += transform.forward * (Time.deltaTime * 10f);
			}
			
			if (timeUntilExplosion <= 0 && !_isExploded)
			{
				_isExploded = true;
				explosion.gameObject.SetActive(true);
				StartCoroutine(ExplosionFlash());
			}
			
			if (_isExploded)
			{
				explosionTime -= Time.deltaTime;
			}
			
			if (explosionTime <= 0)
			{
				Destroy(gameObject);
			}
		}

		private IEnumerator Glitch()
		{
			while (timeUntilExplosion > 0)
			{
				renderer.material.SetFloat("_Glich_strenght", 0f);
				yield return new WaitForSeconds(Random.Range(0.2f, 0.3f));
				renderer.material.SetFloat("_Glich_strenght", Random.Range(0.05f, 0.3f));
				yield return new WaitForSeconds(Random.Range(0.02f, 0.1f));
			}
		}

		private IEnumerator ExplosionFlash()
		{
			var volume = FindObjectOfType<Volume>();
			volume.profile.TryGet(out ShadowsMidtonesHighlights _smh);
			volume.profile.TryGet(out ColorAdjustments _ca);

			_smh.active = true;
			_ca.active = true;
			
			var cinemachineVirtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
			var perlinNoise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			perlinNoise.m_AmplitudeGain = 2;

			yield return new WaitForSeconds(0.1f);
			
			_smh.active = false;
			_ca.active = false;
			
			yield return new WaitForSeconds(0.3f);
			perlinNoise.m_AmplitudeGain = 1.5f;
			yield return new WaitForSeconds(0.5f);
			perlinNoise.m_AmplitudeGain = 1;
			yield return new WaitForSeconds(0.3f);
			perlinNoise.m_AmplitudeGain = 0.5f;
			yield return new WaitForSeconds(0.7f);
			perlinNoise.m_AmplitudeGain = 0f;
			
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Enemy"))
			{
				other.GetComponent<Damageable>().TakeDamage(_playerStatsComponent.GetTotalDamage(10), _elementalWeapon);
			}
		}
	}
}