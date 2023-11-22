using System;
using System.Collections;
using System.Linq;
using Objects.Players.PermUpgrades;
using Objects.Players.Scripts;
using Objects.Stage;
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
                PermUpgradeType.AttackCount,
                PermUpgradeType.BuyGems
            }).ToList().OrderBy(_ => Random.value).FirstOrDefault();
            statIncrease = statToUpgrade.IsPercent() ? (GameData.GetPlayerCharacterRank() >= CharacterRank.E4 ? 0.1f : 0.02f) : (GameData.GetPlayerCharacterRank() >= CharacterRank.E4 ? 2f : 1f);
            playerStatsComponent.Add(statToUpgrade, statIncrease);
        }

        public void Shatter()
        {
            playerStatsComponent.Add(statToUpgrade, -statIncrease);
            if (GameData.GetPlayerCharacterRank() >= CharacterRank.E2)
            {
                playerStatsComponent.TemporaryMoveSpeedBoost(0.5f, 2);
                playerStatsComponent.TemporaryAttackBoost(3f, 2);
            }
            
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
