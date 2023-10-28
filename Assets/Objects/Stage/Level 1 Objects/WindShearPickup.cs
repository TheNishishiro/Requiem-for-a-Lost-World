using System;
using System.Linq;
using DefaultNamespace.Data;
using DefaultNamespace.Data.Achievements;
using Managers;
using UnityEngine;

namespace Objects.Stage.Level_1_Objects
{
    public class WindShearPickup : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(!SaveFile.Instance.IsAchievementUnlocked(AchievementEnum.UnlockWindShear));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SaveFile.Instance.UnlockAchievement(AchievementEnum.UnlockWindShear);
                gameObject.SetActive(false);
            }
        }
    }
}