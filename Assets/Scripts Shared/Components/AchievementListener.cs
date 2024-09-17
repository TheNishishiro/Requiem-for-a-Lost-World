using DefaultNamespace.Data.Achievements;
using Managers;
using UnityEngine;

namespace DefaultNamespace.Components
{
    public class AchievementListener : MonoBehaviour
    {
        public void UnlockAchievement(int achievementId)
        {
            AchievementManager.instance.UnlockAchievement((AchievementEnum)achievementId);
        }
    }
}