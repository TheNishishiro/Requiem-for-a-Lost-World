using System;
using System.Collections;
using Objects.Players.PermUpgrades;
using Objects.Players.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects.Characters.Amelia.Skill
{
    public class AmeliaGlassShard : MonoBehaviour
    {
        private float rotationSpeed;
        private float radius;
        private Vector3 desiredPosition;
        private Vector3 rotationAxis;   
        private Transform parentTransform;
        private PermUpgradeType statToUpgrade;
        private float statIncrease;
        private PlayerStatsComponent playerStatsComponent;
        
        public void Initialize(Transform parent)
        {
            playerStatsComponent = FindObjectOfType<PlayerStatsComponent>();
            parentTransform = parent;
            rotationSpeed = Random.Range(60, 130);
            radius = Random.Range(0.1f, 0.3f);
            rotationAxis = Random.onUnitSphere;
            transform.position = (transform.position - parentTransform.position).normalized * radius + parentTransform.position;

            statToUpgrade = (PermUpgradeType) Random.Range(0, Enum.GetValues(typeof(PermUpgradeType)).Length);
            statIncrease = statToUpgrade.IsPercent() ? Random.Range(0.01f, 0.1f) : Random.Range(1, 3);
            playerStatsComponent.Add(statToUpgrade, statIncrease);
        }

        public void Shatter()
        {
            playerStatsComponent.Add(statToUpgrade, -statIncrease);
            Destroy(gameObject);
        }
	
        void Update () {
            transform.RotateAround (parentTransform.position, rotationAxis, rotationSpeed * Time.deltaTime);
            desiredPosition = (transform.position - parentTransform.position).normalized * radius + parentTransform.position;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * rotationSpeed);
        }
    }
}
