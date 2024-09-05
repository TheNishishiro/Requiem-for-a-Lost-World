using System;
using DefaultNamespace.Data.Achievements;
using Managers;
using UnityEngine;

namespace DefaultNamespace.Components
{
    [RequireComponent(typeof(BoxCollider))]
    public class AchievementUnlocker : MonoBehaviour
    {
        [SerializeField] private AchievementEnum achievement;
        
        private void OnTriggerEnter(Collider other)
        {
            AchievementManager.instance.UnlockAchievement(achievement);
        }
    }
}