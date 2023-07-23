using System;
using System.Collections;
using System.Linq;
using Objects.Players.PermUpgrades;
using Objects.Players.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects.Characters.Amelia.Skill
{
    public class AmeliaGlassShard : MonoBehaviour
    {
        [SerializeField] private GameObject shatterEffect;
        [SerializeField] private GameObject glashShardModel;
        private float rotationSpeed;
        private float radius;
        private Vector3 desiredPosition;
        private Vector3 rotationAxis;   
        private Transform parentTransform;
        private PermUpgradeType statToUpgrade;
        private float statIncrease;
        private PlayerStatsComponent playerStatsComponent;
        private float e4ExperienceIncreaseAmount = 0.02f;
        
        public void Initialize(Transform parent)
        {
            playerStatsComponent = FindObjectOfType<PlayerStatsComponent>();
            parentTransform = parent;
            rotationSpeed = Random.Range(60, 130);
            radius = Random.Range(0.1f, 0.3f);
            rotationAxis = Random.onUnitSphere;
            transform.position = (transform.position - parentTransform.position).normalized * radius + parentTransform.position;

            statToUpgrade = Enum.GetValues(typeof(PermUpgradeType)).Cast<PermUpgradeType>().Except(new[]
            {
                PermUpgradeType.AttackCount
            }).ToList().OrderBy(_ => Random.value).FirstOrDefault();
            statIncrease = statToUpgrade.IsPercent() ? 0.01f : 1;
            playerStatsComponent.Add(statToUpgrade, statIncrease);
            playerStatsComponent.IncreaseExperienceGain(e4ExperienceIncreaseAmount);
        }

        public void Shatter()
        {
            playerStatsComponent.Add(statToUpgrade, -statIncrease);
            playerStatsComponent.TemporaryMoveSpeedBoost(0.2f, 2);
            playerStatsComponent.TemporaryAttackBoost(0.3f, 2);
            playerStatsComponent.IncreaseExperienceGain(-e4ExperienceIncreaseAmount);
            StartCoroutine(PlayShatterEffect());
        }
	
        void Update () {
            transform.RotateAround (parentTransform.position, rotationAxis, rotationSpeed * Time.deltaTime);
            desiredPosition = (transform.position - parentTransform.position).normalized * radius + parentTransform.position;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * rotationSpeed);
        }

        private IEnumerator PlayShatterEffect()
        {
            glashShardModel.SetActive(false);
            shatterEffect.SetActive(true);
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}
