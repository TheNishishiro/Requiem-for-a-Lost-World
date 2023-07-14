using System;
using System.Collections;
using DefaultNamespace;
using Managers;
using Objects.Stage;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Objects.Enemies.Peace_Ender
{
	public class PEAttackComponent : MonoBehaviour
	{
		private ChaseComponent _chaseComponent;
		[SerializeField] private GameObject lightPillarPrefab;
		[SerializeField] private GameObject indicatorPrefab;
		[SerializeField] private int lightPillarCount;
		[SerializeField] private float lightPillarAttackDelay;
		[SerializeField] private float lightPillarAttackArea;
		[SerializeField] private float attackCooldown;
		private float _currentCooldown;

		private void Awake()
		{
			_chaseComponent = GetComponentInParent<ChaseComponent>();
		}
		
		public void Update()
		{
			_currentCooldown -= Time.deltaTime;
			if (_currentCooldown <= 0)
			{
				_currentCooldown = attackCooldown;
				LightPillarAttack();
			}
		}

		public void LightPillarAttack()
		{
			for (var i = 0; i < lightPillarCount; i++)
			{
				var point = Utilities.GetRandomInAreaFreezeParameter(transform.position, lightPillarAttackArea, isFreezeY: true);
				var attackPosition = Utilities.GetPointOnColliderSurface(point, transform, 0.2f); 
				
				var indicator = SpawnManager.instance.SpawnObject(attackPosition, indicatorPrefab);
                StartCoroutine(SpawnPillar(indicator, attackPosition));
			}
		}
		
		private IEnumerator SpawnPillar(Object indicator, Vector3 position)
		{
			yield return new WaitForSeconds(lightPillarAttackDelay);
			Destroy(indicator);
			Instantiate(lightPillarPrefab, position, Quaternion.identity);
		}
	}
}