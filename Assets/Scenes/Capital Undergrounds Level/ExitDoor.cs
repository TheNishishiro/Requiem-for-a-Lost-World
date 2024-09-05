using DefaultNamespace.Data.Achievements;
using Managers;
using UnityEngine;

namespace Scenes.Capital_Undergrounds_Level
{
    public class ExitDoor : MonoBehaviour
    {
        public void ExitDungeon()
        {
            AchievementManager.instance.UnlockAchievement(AchievementEnum.IsThisTheWayOut);
            GameManager.instance.BackToMainMenu(false);
        }
    }
}